(function($){
    $(function(){
        var publicHeader = $('.public-header');

        (function(){
            /*鼠标经过显示下拉菜单*/
            publicHeader.on('mouseenter mouseleave', '.c-a-wrap', function(e){
                var _self = $(this);
                var cawShow = _self.find('.caw-show');

                if (e.type == 'mouseenter'){
                    _self.addClass('c-a-wrap-hover');
                    cawShow.show();
                } else if (e.type == 'mouseleave'){
                    _self.removeClass('c-a-wrap-hover');
                    cawShow.hide();
                }
            });

           
        })();
    });
})(jQuery);