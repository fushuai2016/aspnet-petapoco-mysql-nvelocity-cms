using System;
using System.Collections.Generic;
using System.Text;

namespace ccphl.Common
{
    public class DTEnums
    {
        /// <summary>
        /// 统一管理操作枚举
        /// </summary>
        public enum ActionEnum
        {
            /// <summary>
            /// 所有
            /// </summary>
            All=0,
            /// <summary>
            /// 显示
            /// </summary>
            Show=1,
            /// <summary>
            /// 查看
            /// </summary>
            View=2,
            /// <summary>
            /// 添加
            /// </summary>
            Add=4,
            /// <summary>
            /// 修改
            /// </summary>
            Edit=8,
            /// <summary>
            /// 删除
            /// </summary>
            Delete=16,
            /// <summary>
            /// 审核
            /// </summary>
            Audit=32,
            /// <summary>
            /// 回复
            /// </summary>
            Reply=64,
            /// <summary>
            /// 确认
            /// </summary>
            Confirm=128,
            /// <summary>
            /// 取消
            /// </summary>
            Cancel=256,
            /// <summary>
            /// 作废
            /// </summary>
            Invalid=512,
            /// <summary>
            /// 生成
            /// </summary>
            Build=1024,
            /// <summary>
            /// 安装
            /// </summary>
            Instal=2048,
            /// <summary>
            /// 卸载
            /// </summary>
            UnLoad=4096,
            /// <summary>
            /// 备份
            /// </summary>
            Back=8192,
            /// <summary>
            /// 还原
            /// </summary>
            Restore=16384,
            /// <summary>
            /// 替换
            /// </summary>
            Replace=32768,
             /// <summary>
            /// 登录
            /// </summary>
            Login=-1
        }

        /// <summary>
        /// 系统导航菜单类别枚举
        /// </summary>
        public enum NavigationEnum
        {
            /// <summary>
            /// 系统后台菜单
            /// </summary>
            System,
            /// <summary>
            /// 会员中心导航
            /// </summary>
            Users,
            /// <summary>
            /// 网站主导航
            /// </summary>
            WebSite
        }

        /// <summary>
        /// 用户生成码枚举
        /// </summary>
        public enum CodeEnum
        {
            /// <summary>
            /// 注册验证
            /// </summary>
            RegVerify,
            /// <summary>
            /// 邀请注册
            /// </summary>
            Register,
            /// <summary>
            /// 取回密码
            /// </summary>
            Password
        }

        /// <summary>
        /// 金额类型枚举
        /// </summary>
        public enum AmountTypeEnum
        {
            /// <summary>
            /// 系统赠送
            /// </summary>
            SysGive,
            /// <summary>
            /// 在线充值
            /// </summary>
            Recharge,
            /// <summary>
            /// 用户消费
            /// </summary>
            Consumption,
            /// <summary>
            /// 购买商品
            /// </summary>
            BuyGoods,
            /// <summary>
            /// 积分兑换
            /// </summary>
            Convert
        }
    }
}
