using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Search;
using System;
using Lucene.Net.Analysis;
using PanGu;

namespace ccphl.Web.UI
{
    public class LuceneHelper {
        public static readonly LuceneHelper luceneHelper = new LuceneHelper();
        public static readonly string indexPath = HttpContext.Current.Server.MapPath("~/IndexData");
        public LuceneHelper()
        {
        }
        //请求队列 解决索引目录同时操作的并发问题
        private Queue<BookViewMode> bookQueue = new Queue<BookViewMode>();
        /// <summary>
        /// 新增Books表信息时 添加邢增索引请求至队列
        /// </summary>
        /// <param name="books"></param>
        public void Add(Books books) {
            BookViewMode bvm = new BookViewMode();
            bvm.Id = books.Id;
            bvm.Title = books.Title;
            bvm.Content = books.Content;
            bvm.Link = books.Link;
            bvm.Thumbnail = books.Thumbnail;
            bvm.AddTime = books.AddTime;
            bvm.IT = IndexType.Insert;
            bookQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 删除Books表信息时 添加删除索引请求至队列
        /// </summary>
        /// <param name="bid"></param>
        public void Del(int bid) {
            BookViewMode bvm = new BookViewMode();
            bvm.Id = bid;
            bvm.IT = IndexType.Delete;
            bookQueue.Enqueue(bvm);
        }
        /// <summary>
        /// 修改Books表信息时 添加修改索引(实质上是先删除原有索引 再新增修改后索引)请求至队列
        /// </summary>
        /// <param name="books"></param>
        public void Mod(Books books) {
            BookViewMode bvm = new BookViewMode();
            bvm.Id = books.Id;
            bvm.Title = books.Title;
            bvm.Content = books.Content;
            bvm.Link = books.Link;
            bvm.Thumbnail = books.Thumbnail;
            bvm.AddTime = books.AddTime;
            bvm.IT = IndexType.Modify;
            bookQueue.Enqueue(bvm);
        }

        public void StartNewThread() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(QueueToIndex));
        }

        //定义一个线程 将队列中的数据取出来 插入索引库中
        private void QueueToIndex(object para) {
            while(true) {
                if(bookQueue.Count > 0) {
                    CRUDIndex();
                } else {
                    Thread.Sleep(3000);
                }
            }
        }
        /// <summary>
        /// 更新索引库操作
        /// </summary>
        private void CRUDIndex() {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            bool isExist = IndexReader.IndexExists(directory);
            if(isExist) {
                if(IndexWriter.IsLocked(directory)) {
                    IndexWriter.Unlock(directory);
                }
            }
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);
            while(bookQueue.Count > 0) {
                Document document = new Document();
                BookViewMode book = bookQueue.Dequeue();
                if(book.IT == IndexType.Insert) {
                    document.Add(new Field("id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", book.Title, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("content", book.Content, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("link", book.Link, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("thumbnail", book.Thumbnail, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("addtime", book.AddTime, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    writer.AddDocument(document);
                } else if(book.IT == IndexType.Delete) {
                    writer.DeleteDocuments(new Term("id", book.Id.ToString()));
                } else if(book.IT == IndexType.Modify) {
                    //先删除 再新增
                    writer.DeleteDocuments(new Term("id", book.Id.ToString()));
                    document.Add(new Field("id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("title", book.Title, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("content", book.Content, Field.Store.YES, Field.Index.ANALYZED,
                                           Field.TermVector.WITH_POSITIONS_OFFSETS));
                    document.Add(new Field("link", book.Link, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("thumbnail", book.Thumbnail, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    document.Add(new Field("addtime", book.AddTime, Field.Store.YES, Field.Index.NOT_ANALYZED));
                    writer.AddDocument(document);
                }
            }
            writer.Close();
            directory.Close();
        }
        /// <summary>
        /// 创建全文索引
        /// </summary>
        /// <param name="blist">插入的数据集合</param>
        public void CreateIndexByData(List<Books> blist)
        {       
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NativeFSLockFactory());
            //IndexReader:对索引库进行读取的类
            bool isExist = IndexReader.IndexExists(directory); //是否存在索引库文件夹以及索引库特征文件
            if (isExist)
            {
                //如果索引目录被锁定（比如索引过程中程序异常退出或另一进程在操作索引库），则解锁
                //Q:存在问题 如果一个用户正在对索引库写操作 此时是上锁的 而另一个用户过来操作时 将锁解开了 于是产生冲突 --解决方法后续
                if (IndexWriter.IsLocked(directory))
                {
                    IndexWriter.Unlock(directory);
                }
            }

            //创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)
            //补充:使用IndexWriter打开directory时会自动对索引库文件上锁
            IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isExist, IndexWriter.MaxFieldLength.UNLIMITED);

            List<Books> bookList = blist;

            //--------------------------------遍历数据源 将数据转换成为文档对象 存入索引库
            foreach (var book in bookList)
            {
                Document document = new Document(); //new一篇文档对象 --一条记录对应索引库中的一个文档

                //向文档中添加字段  Add(字段,值,是否保存字段原始值,是否针对该列创建索引)
                document.Add(new Field("id", book.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));//--所有字段的值都将以字符串类型保存 因为索引库只存储字符串类型数据

                //Field.Store:表示是否保存字段原值。指定Field.Store.YES的字段在检索时才能用document.Get取出原值  
                //Field.Index.NOT_ANALYZED:指定不按照分词后的结果保存--是否按分词后结果保存取决于是否对该列内容进行模糊查询


                document.Add(new Field("title", book.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                //Field.Index.ANALYZED:指定文章内容按照分词后结果保存 否则无法实现后续的模糊查询 
                //WITH_POSITIONS_OFFSETS:指示不仅保存分割后的词 还保存词之间的距离

                document.Add(new Field("content", book.Content, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS));

                document.Add(new Field("link", book.Link, Field.Store.YES, Field.Index.NOT_ANALYZED));

                document.Add(new Field("thumbnail", book.Thumbnail, Field.Store.YES, Field.Index.NOT_ANALYZED));
                document.Add(new Field("addtime", book.AddTime, Field.Store.YES, Field.Index.NOT_ANALYZED));
                writer.AddDocument(document); //文档写入索引库
            }
            writer.Close();//会自动解锁
            directory.Close(); //不要忘了Close，否则索引结果搜不到
        }

        /// <summary>
        /// 从索引库中检索关键字
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="size">显示数量</param>
        /// <param name="keywords">关键词</param>
        /// <param name="total">结果数据总数量</param>
        /// <returns></returns>
        public List<Books> Search(int page, int size, string keywords, out int total)
        {
            FSDirectory directory = FSDirectory.Open(new DirectoryInfo(indexPath), new NoLockFactory());
            IndexReader reader = IndexReader.Open(directory, true);
            IndexSearcher searcher = new IndexSearcher(reader);
            //关键词Or关系设置
            BooleanQuery queryOr = new BooleanQuery();
            TermQuery query = null;
            foreach (string word in SplitWords(keywords))
            {
                query = new TermQuery(new Term("title", word));
                queryOr.Add(query, BooleanClause.Occur.SHOULD);//这里设置 条件为Or关系
                query = new TermQuery(new Term("content", word));
                queryOr.Add(query, BooleanClause.Occur.SHOULD);//这里设置 条件为Or关系
            }
            TopScoreDocCollector collector = TopScoreDocCollector.create(1000, true);
            Sort sort = new Sort();
            SortField f = new SortField("addtime", SortField.STRING, true); // 按照createdate字段排序，true表示降序
            sort.SetSort(f);
            searcher.Search(queryOr, null, collector);//根据query查询条件进行查询，查询结果放入collector容器

            //TopDocs 指定0到GetTotalHits() 即所有查询结果中的文档 如果TopDocs(20,10)则意味着获取第20-30之间文档内容 达到分页的效果
            ScoreDoc[] docs = collector.TopDocs(0, collector.GetTotalHits()).scoreDocs;
            //集合总数
            total = collector.GetTotalHits();
            //集合读取开始位置
            int start = (page - 1) * size;
            //集合读取结束位置
            int end = page * size;
            //展示数据实体对象集合
            List<Books> bookResult = new List<Books>();
            for (int i = 0; i < docs.Length; i++)
            {
                if (i >= start && i < end)
                {
                    int docId = docs[i].doc;//得到查询结果文档的id（Lucene内部分配的id）
                    Document doc = searcher.Doc(docId);//根据文档id来获得文档对象Document
                    Books book = new Books();
                    book.Title = HightLight(keywords, doc.Get("title"));
                    //搜索关键字高亮显示 使用盘古提供高亮插件
                    book.Content = HightLight(keywords, doc.Get("content"));
                    book.Id = Convert.ToInt32(doc.Get("id"));
                    book.Link = doc.Get("link");
                    book.Thumbnail = doc.Get("thumbnail");
                    book.AddTime = doc.Get("addtime");
                    bookResult.Add(book);
                }
            }
            return bookResult;
        }
        private static string[] SplitWords(string content)
        {
            List<string> strList = new List<string>();
            Analyzer analyzer = new PanGuAnalyzer();//指定使用盘古 PanGuAnalyzer 分词算法
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(content));
            Lucene.Net.Analysis.Token token = null;
            while ((token = tokenStream.Next()) != null)
            { //Next继续分词 直至返回null
                strList.Add(token.TermText()); //得到分词后结果
            }
            return strList.ToArray();
        }

        //需要添加PanGu.HighLight.dll的引用
        /// <summary>
        /// 搜索结果高亮显示
        /// </summary>
        /// <param name="keyword"> 关键字 </param>
        /// <param name="content"> 搜索结果 </param>
        /// <returns> 高亮后结果 </returns>
        private static string HightLight(string keyword, string content)
        {
            //创建HTMLFormatter,参数为高亮单词的前后缀
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter =
                new PanGu.HighLight.SimpleHTMLFormatter("<font style=\"font-style:normal;color:#cc0000;\"><b>", "</b></font>");
            //创建 Highlighter ，输入HTMLFormatter 和 盘古分词对象Semgent
            PanGu.HighLight.Highlighter highlighter =
                            new PanGu.HighLight.Highlighter(simpleHTMLFormatter,
                            new Segment());
            //设置每个摘要段的字符数
            highlighter.FragmentSize = 1000;
            //获取最匹配的摘要段
            return highlighter.GetBestFragment(keyword, content);
        }
    }

    public class Books
    {
        public int Id
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Content
        {
            get;
            set;
        }
        public string Link
        {
            get;
            set;
        }
        public string Thumbnail
        {
            get;
            set;
        }
        public string AddTime
        {
            get;
            set;
        }
    }
    public class BookViewMode {
        public int Id {
            get;
            set;
        }
        public string Title {
            get;
            set;
        }
        public string Content {
            get;
            set;
        }
        public string Link
        {
            get;
            set;
        }
        public string Thumbnail
        {
            get;
            set;
        }
        public string AddTime
        {
            get;
            set;
        }
        public IndexType IT {
            get;
            set;
        }
    }
    //操作类型枚举
    public enum IndexType {
        Insert,
        Modify,
        Delete
    }
}