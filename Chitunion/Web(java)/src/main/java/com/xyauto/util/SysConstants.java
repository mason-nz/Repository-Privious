package com.xyauto.util;

public interface SysConstants {

	String SEESION_USER = "userInfo";
	String COOKIE_NAME = "ct-uinfo";
	String LOGIN_URL = "http://www.chitunion.com/Login.aspx";
	String EXXT_URL = "http://www.chitunion.com/exit.aspx";
	String GET_USER_INFO = "http://www.chitunion.com/api/Authorize/GetLoginInfo";
	String CHECK_RIGHT = "http://www.chitunion.com/api/Authorize/CheckRight";
	/**
	 * 最大上传限制
	 */
	Integer MAX_UPLOAD_SIZE = 10 * 1024 * 1024;
	/**
	 * 图片最大限制
	 */
	Integer MAX_UPLOAD_IMG = 2 * 1024 * 1024;
	/**
	 * 上传枚举 账号管理 营业执照
	 */
	Integer UPLOAD_USER_BL = 33005;
	/**
	 * 上传枚举 账号管理 组织机构代码证
	 */
	Integer UPLOAD_USER_O = 33006;
	/**
	 * 上传枚举 账号管理 身份证
	 */
	Integer UPLOAD_USER_IDC = 33007;
	/**
	 * 用户信息详情表
	 */
	String USER_DETAIL_INFO = "UserDetailInfo";
	/**
	 * 上传格式jpg
	 */
	String JPG = "ffd8ff";
	/**
	 * 上传格式png
	 */
	String PNG = "89504e47";
	/**
	 * 上传格式gif
	 */
	String GIF = "47494638";
	/**
	 * SysID
	 */
	String SYS_ID = "SYS001";
	/**
	 * 17005 17 标签管理
	 */
	Integer TITLE_MANAGER = 17005;
	/**
	 * 17006 17 角色权限管理
	 */
	Integer USER_ROLE_MANAGER = 17006;
	/**
	 * 17001 17 账号管理
	 */
	Integer USER_ACCOUNT_MANAGER = 17001;
	/**
	 * 1-新增
	 */
	Integer INSERT = 1;
	/**
	 * 2-删除
	 */
	Integer DELETE = 2;
	/**
	 * 3-修改
	 */
	Integer UPDATE = 3;
	/**
	 * 4-查询
	 */
	Integer SELECT = 4;
	/**
	 * 来源-自营
	 */
	Integer SOURCE_1 = 3001;
	/**
	 * 来源-自助
	 */
	Integer SOURCE_2 = 3002;
	/**
	 * 状态-正常
	 */
	Integer STATUS_0 = 0;
	/**
	 * 状态-禁用
	 */
	Integer STATUS_1 = 1;
	/**
	 * 状态-企业
	 */
	Integer TYPE_1 = 1001;
	/**
	 * 状态-个人
	 */
	Integer TYPE_2 = 1002;
	String RECOVER_PWD_MESSAGE = "您正在重置密码，验证码：X,请在15分钟内按页面提示提交验证码，切勿将验证码泄露给他人。";
	String RESET_PASS_MESSAGE = "您的密码已成功重置，新密码为 “X”，登录后请尽快修改，以保证账户的安全！";
	String PASS_MESSAGE = "您正在修改您的联系方式，验证码为：X；有效期3分钟，过期无效；如非本人操作，请忽略本短信。";
	String SMS_MESSAGE = "亲，欢迎注册赤兔联盟，您的手机验证码：X；有效期3分钟，过期无效。";
	String LOGINPWDKEY = "1234567890ABC";
	String MD5CHARTSET = "UTF-8";
	/**
	 * 分页单位
	 */
	Integer PAGEROWS = 20;
	/**
	 * 角色：超级管理员
	 */
	String ROLE_MANAGER = "SYS001RL00001";
	/**
	 * 角色：广告主
	 */
	String ROLE_ADD = "SYS001RL00002";
	/**
	 * 角色：媒体主
	 */
	String ROLE_MEDIA = "SYS001RL00003";
	/**
	 * 角色：运营
	 */
	String ROLE_OPERATE = "SYS001RL00004";
	/**
	 * 角色：AE
	 */
	String ROLE_AE = "SYS001RL00005";
	/**
	 * 角色：策划
	 */
	String ROLE_PLAN = "SYS001RL00006";
	/**
	 * 广告主
	 */
	String CATEGORY_1 = "29001";
	/**
	 * 媒体主
	 */
	String CATEGORY_2 = "29002";
	/**
	 * 标签模块分页的条数
	 */
	Integer TITLE_PAGEROWS = 20;

	/**
	 * 媒体分页单位
	 */
	Integer MEDIA_PAGEROWS = 10;

}