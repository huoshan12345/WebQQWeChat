namespace iQQ.Net.WebQQCore.Im.Core
{

    public abstract class QQConstants
    {
        public const string APPID = "501004106";
        public const string JSVER = "10153";
        public const string REFFER = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2";
        public const string REFERER_S = "http://s.web2.qq.com/proxy.html?v=20130916001&callback=1&id=1";
        public const string ORIGIN = "http://d1.web2.qq.com";
        public const string ORIGIN_S = "http://s.web2.qq.com";
        public const string USER_AGENT = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Safari/537.36";
        public const string URL_CHECK_VERIFY = "https://ssl.ptlogin2.qq.com/check";
        public const string URL_GET_CAPTCHA = "http://captcha.qq.com/getimage";
        public const string URL_GET_QRCODE = "https://ssl.ptlogin2.qq.com/ptqrshow";
        public const string URL_CHECK_QRCODE = "https://ssl.ptlogin2.qq.com/ptqrlogin";
        public const string REGXP_CHECK_VERIFY = "ptui_checkVC\\('(.*?)','(.*?)','(.*?)'(,\\s*'(.*?)')?\\)";
        public const string REGXP_LOGIN = "ptuiCB\\('(\\d+)','(\\d+)','(.*?)','(\\d+)','(.*?)', '(.*?)'\\)";
        public const string REGXP_JSON_SINGLE_RESULT = "\\{([\\s\\S]*)\\}";
        public const string URL_UI_LOGIN = "https://ssl.ptlogin2.qq.com/login";
        public const string URL_CHANNEL_LOGIN = "http://d1.web2.qq.com/channel/login2";
        public const string URL_GET_FRIEND_INFO = "http://s.web2.qq.com/api/get_friend_info2";
        public const string URL_GET_STRANGER_INFO = "http://s.web2.qq.com/api/get_stranger_info2";
        public const string URL_GET_IMAGE = "https://ssl.captcha.qq.com/getimage";
        public const string URL_POLL_MSG = "http://d1.web2.qq.com/channel/poll2";
        public const string URL_GET_USER_CATEGORIES = "http://s.web2.qq.com/api/get_user_friends2";
        public const string URL_GET_USER_FACE = "http://face1.qun.qq.com/cgi/svr/face/getface";
        public const string URL_GET_GROUP_NAME_LIST = "http://s.web2.qq.com/api/get_group_name_list_mask2";
        public const string URL_GET_USER_ACCOUNT = "http://s.web2.qq.com/api/get_friend_uin2";
        public const string URL_GET_USER_SIGN = "http://s.web2.qq.com/api/get_single_long_nick2";
        public const string URL_GET_ONLINE_BUDDY_LIST = "http://d1.web2.qq.com/channel/get_online_buddies2";
        public const string URL_SEND_BUDDY_MSG = "http://d1.web2.qq.com/channel/send_buddy_msg2";
        public const string URL_SEND_GROUP_MSG = "http://d1.web2.qq.com/channel/send_qun_msg2";
        public const string URL_SEND_DISCUZ_MSG = "http://d1.web2.qq.com/channel/send_discu_msg2";
        public const string URL_SEND_SESSION_MSG = "http://d1.web2.qq.com/channel/send_sess_msg2";
        public const string URL_UPLOAD_OFFLINE_PICTURE = "http://weboffline.ftn.qq.com/ftn_access/upload_offline_pic";
        public const string URL_UPLOAD_CUSTOM_FACE = "http://up.web2.qq.com/cgi-bin/cface_upload";
        public const string URL_CUSTOM_FACE_SIG = "http://d1.web2.qq.com/channel/get_gface_sig2";
        public const string URL_LOGOUT = "http://d1.web2.qq.com/channel/Logout2";
        public const string URL_CHANGE_STATUS = "http://d1.web2.qq.com/channel/change_status2";
        public const string URL_GET_GROUP_INFO_EXT = "http://s.web2.qq.com/api/get_group_info_ext2";
        public const string URL_GROUP_MESSAGE_FILTER = "http://cgi.web2.qq.com/keycgi/qqweb/uac/messagefilter.do";
        public const string URL_GET_DISCUZ_LIST = "http://s.web2.qq.com/api/get_discus_list";
        public const string URL_GET_DISCUZ_INFO = "http://d1.web2.qq.com/channel/get_discu_info";
        public const string URL_GET_RECENT_LIST = "http://d1.web2.qq.com/channel/get_recent_list2";
        public const string URL_SHAKE_WINDOW = "http://d1.web2.qq.com/channel/shake2";
        public const string URL_GET_OFFPIC = "http://d1.web2.qq.com/channel/get_offpic2";
        public const string URL_GET_CFACE2 = "http://d1.web2.qq.com/channel/get_cface2";
        public const string URL_GET_GROUP_PIC = "http://web.qq.com/cgi-bin/get_group_pic";
        public const string URL_GET_SESSION_MSG_SIG = "http://d1.web2.qq.com/channel/get_c2cmsg_sig2";
        public const string URL_SEND_INPUT_NOTIFY = "http://d1.web2.qq.com/channel/input_notify2";
        public const string URL_GET_USER_LEVEL = "http://s.web2.qq.com/api/get_qq_level2";
        public const string URL_GET_GROUP_MEMBER_STATUS = "http://s.web2.qq.com/api/get_group_member_stat2";
        public const string URL_SERACH_USER_BY_UIN = "http://s.web2.qq.com/api/search_qq_by_uin2";
        public const string URL_ADD_NO_VERIFY2 = "http://s.web2.qq.com/api/add_no_verify2";
        public const string URL_ADD_NEED_VERIFY2 = "http://s.web2.qq.com/api/add_need_verify2";
        public const string URL_ADD_ANSWER_AND_ADD = "http://s.web2.qq.com/api/answer_and_add2";
        public const string URL_LOGIN_PAGE = "https://ui.ptlogin2.qq.com/cgi-bin/login?daid=164&target=self&style=16&mibao_css=m_webqq&appid=" + APPID + "&enable_qlogin=0&no_verifyimg=1&s_url=http%3A%2F%2Fw.qq.com%2Fproxy.html&f_url=loginerroralert&strong_login=1&login_state=10&t=20131024001";
        public const string URL_SEARCH_GROUP_INFO = "http://cgi.web2.qq.com/keycgi/qqweb/group/search.do";
        public const string REGXP_LOGIN_SIG = "var g_login_sig=encodeURIComponent\\(\"(.*?)\"\\);";
        public const string URL_EMAIL_POLL = "http://wp.mail.qq.com/poll";
        public const string URL_PT4_AUTH = "http://ptlogin2.qq.com/pt4_auth";
        public const string URL_GET_WP_KEY = "http://mail.qq.com/cgi-bin/getwpkey";
        public const string URL_LOGIN_EMAIL = "http://mail.qq.com/cgi-bin/login?fun=passport&from=webqq";
        public const string URL_MARK_EMAIL = "http://mail.qq.com/cgi-bin/mail_mgr";
        public const string URL_GET_SELF_INFO = "http://s.web2.qq.com/api/get_self_info2";
        public const string REGXP_EMAIL_AUTH = "ptui_auth_CB\\('(.*?)','(.*?)'\\)";
        public const string URL_ACCEPET_BUDDY_ADD = "http://s.web2.qq.com/api/allow_and_add2";

        public const int MAX_POLL_ERR_CNT = 10;
        public const int MAX_RETRY_TIMES = 1;
        public const int HTTP_TIME_OUT = 80000;

        public const string URL_ROBOT_TULING = "http://www.tuling123.com/openapi/api";
        public const string ROBOT_TULING_KEY = "a94f7f3e48c331e86316b9522e552f80";

        public const string URL_ROBOT_MOLI = "http://i.itpk.cn/api.php";
        public const string ROBOT_MOLI_KEY = "b70d2e4162c3683c64a145b3806bad68";
        public const string ROBOT_MOLI_SECRET = "x1yeyia5wrq2";
    }
}
