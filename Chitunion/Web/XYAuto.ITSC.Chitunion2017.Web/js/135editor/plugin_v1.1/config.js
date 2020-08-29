//{
//	"imageActionName":"uploadimage","imageFieldName":"upload","imageMaxSize":2048000,
//	"imageAllowFiles":[".png",".jpg",".jpeg",".gif",".bmp"],"imageCompressEnable":true,"imageCompressBorder":1600,
//	"imageInsertAlign":"none","imageUrlPrefix":"","imagePathFormat":"\/ueditor\/php\/upload\/image\/{yyyy}{mm}{dd}\/{time}{rand:6}",
//	"scrawlActionName":"uploadscrawl","scrawlFieldName":"upload","scrawlPathFormat":"\/ueditor\/php\/upload\/image\/{yyyy}{mm}{dd}\/{time}{rand:6}",
//	"scrawlMaxSize":2048000,"scrawlUrlPrefix":"","scrawlInsertAlign":"none","snapscreenActionName":"uploadimage",
//	"snapscreenPathFormat":"\/ueditor\/php\/upload\/image\/{yyyy}{mm}{dd}\/{time}{rand:6}","snapscreenUrlPrefix":"","snapscreenInsertAlign":"none",
//	"catcherLocalDomain":["image1.135editor.com","image2.135editor.com","img.wx135.com","cdn.135editor.com","img.baidu.com"],
//	"catcherUrl":"http://upload.135editor.com/uploadfiels/ueditor?action=catchimage","catcherActionName":"catchimage","catcherFieldName":"source",
//	"catcherPathFormat":"\/ueditor\/php\/upload\/image\/{yyyy}{mm}{dd}\/{time}{rand:6}","catcherUrlPrefix":"",
//	"catcherMaxSize":2048000,"catcherAllowFiles":[".png",".jpg",".jpeg",".gif",".bmp"],"videoActionName":"uploadvideo","videoFieldName":"upload",
//	"videoPathFormat":"\/ueditor\/php\/upload\/video\/{yyyy}{mm}{dd}\/{time}{rand:6}","videoUrlPrefix":"","videoMaxSize":2048000,
//	"videoAllowFiles":[".flv",".swf",".mkv",".avi",".rm",".rmvb",".mpeg",".mpg",".ogg",".ogv",".mov",".wmv",".mp4",".webm",".mp3",".wav",".mid"],
//	"fileActionName":"uploadfile","fileFieldName":"upload","filePathFormat":"\/ueditor\/php\/upload\/file\/{yyyy}{mm}{dd}\/{time}{rand:6}",
//	"fileUrlPrefix":"","fileMaxSize":2048000,
//	"fileAllowFiles":[".png",".jpg",".jpeg",".gif",".bmp",".flv",".swf",".mkv",".avi",".rm",".rmvb",".mpeg",".mpg",".ogg",".ogv",".mov",".wmv",".mp4",".webm",".mp3",".wav",".mid",".rar",".zip",".tar",".gz",".7z",".bz2",".cab",".iso",".doc",".docx",".xls",".xlsx",".ppt",".pptx",".pdf",".txt",".md",".xml"],
//	"imageManagerActionName":"listimage","imageManagerListPath":"\/ueditor\/php\/upload\/image\/","imageManagerListSize":20,
//	"imageManagerUrlPrefix":"","imageManagerInsertAlign":"none","imageManagerAllowFiles":[".png",".jpg",".jpeg",".gif",".bmp"],
//	"fileManagerActionName":"listfile","fileManagerListPath":"\/ueditor\/php\/upload\/file\/","fileManagerUrlPrefix":"","fileManagerListSize":20,
//	"fileManagerAllowFiles":[".png",".jpg",".jpeg",".gif",".bmp",".flv",".swf",".mkv",".avi",".rm",".rmvb",".mpeg",".mpg",".ogg",".ogv",".mov",".wmv",".mp4",".webm",".mp3",".wav",".mid",".rar",".zip",".tar",".gz",".7z",".bz2",".cab",".iso",".doc",".docx",".xls",".xlsx",".ppt",".pptx",".pdf",".txt",".md",".xml"]
//}

/* ǰ���ͨ����ص�����,ע��ֻ����ʹ�ö��з�ʽ */
{
    /* �ϴ�ͼƬ������ */
    "imageActionName": "uploadimage", /* ִ���ϴ�ͼƬ��action���� */
    "imageFieldName": "upfile", /* �ύ��ͼƬ������ */
    "imageMaxSize": 2048000, /* �ϴ���С���ƣ���λB */
    "imageAllowFiles": [".png", ".jpg", ".jpeg", ".gif", ".bmp"], /* �ϴ�ͼƬ��ʽ��ʾ */
    "imageCompressEnable": true, /* �Ƿ�ѹ��ͼƬ,Ĭ����true */
    "imageCompressBorder": 1600, /* ͼƬѹ��������� */
    "imageInsertAlign": "none", /* �����ͼƬ������ʽ */
    "imageUrlPrefix": "", /* ͼƬ����·��ǰ׺ */
    "imagePathFormat": "/UploadFiles/{yyyy}/{mm}/{dd}/{hh}/{filename}${rand:32}", /* �ϴ�����·��,�����Զ��屣��·�����ļ�����ʽ */
    /* {filename} ���滻��ԭ�ļ���,����������Ҫע�������������� */
    /* {rand:6} ���滻�������,������������������λ�� */
    /* {time} ���滻��ʱ��� */
    /* {yyyy} ���滻����λ��� */
    /* {yy} ���滻����λ��� */
    /* {mm} ���滻����λ�·� */
    /* {dd} ���滻����λ���� */
    /* {hh} ���滻����λСʱ */
    /* {ii} ���滻����λ���� */
    /* {ss} ���滻����λ�� */
    /* �Ƿ��ַ� \ : * ? " < > | */
    /* �����忴�����ĵ�: fex.baidu.com/ueditor/#use-format_upload_filename */

    /* ͿѻͼƬ�ϴ������� */
    "scrawlActionName": "uploadscrawl", /* ִ���ϴ�Ϳѻ��action���� */
    "scrawlFieldName": "upfile", /* �ύ��ͼƬ������ */
    "scrawlPathFormat": "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}", /* �ϴ�����·��,�����Զ��屣��·�����ļ�����ʽ */
    "scrawlMaxSize": 2048000, /* �ϴ���С���ƣ���λB */
    "scrawlUrlPrefix": "/ueditor/net/", /* ͼƬ����·��ǰ׺ */
    "scrawlInsertAlign": "none",

    /* ��ͼ�����ϴ� */
    "snapscreenActionName": "uploadimage", /* ִ���ϴ���ͼ��action���� */
    "snapscreenPathFormat": "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}", /* �ϴ�����·��,�����Զ��屣��·�����ļ�����ʽ */
    "snapscreenUrlPrefix": "/ueditor/net/", /* ͼƬ����·��ǰ׺ */
    "snapscreenInsertAlign": "none", /* �����ͼƬ������ʽ */

    /* ץȡԶ��ͼƬ���� */
    "catcherLocalDomain": ["127.0.0.1", "localhost", "img.baidu.com"],
    "catcherActionName": "catchimage", /* ִ��ץȡԶ��ͼƬ��action���� */
    "catcherFieldName": "source", /* �ύ��ͼƬ�б������ */
    "catcherPathFormat": "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}", /* �ϴ�����·��,�����Զ��屣��·�����ļ�����ʽ */
    "catcherUrlPrefix": "/ueditor/net/", /* ͼƬ����·��ǰ׺ */
    "catcherMaxSize": 2048000, /* �ϴ���С���ƣ���λB */
    "catcherAllowFiles": [".png", ".jpg", ".jpeg", ".gif", ".bmp"], /* ץȡͼƬ��ʽ��ʾ */

    /* �ϴ���Ƶ���� */
    "videoActionName": "uploadvideo", /* ִ���ϴ���Ƶ��action���� */
    "videoFieldName": "upfile", /* �ύ����Ƶ������ */
    "videoPathFormat": "upload/video/{yyyy}{mm}{dd}/{time}{rand:6}", /* �ϴ�����·��,�����Զ��屣��·�����ļ�����ʽ */
    "videoUrlPrefix": "/ueditor/net/", /* ��Ƶ����·��ǰ׺ */
    "videoMaxSize": 102400000, /* �ϴ���С���ƣ���λB��Ĭ��100MB */
    "videoAllowFiles": [
        ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid"], /* �ϴ���Ƶ��ʽ��ʾ */

    /* �ϴ��ļ����� */
    "fileActionName": "uploadfile", /* controller��,ִ���ϴ���Ƶ��action���� */
    "fileFieldName": "upfile", /* �ύ���ļ������� */
    "filePathFormat": "upload/file/{yyyy}{mm}{dd}/{time}{rand:6}", /* �ϴ�����·��,�����Զ��屣��·�����ļ�����ʽ */
    "fileUrlPrefix": "/ueditor/net/", /* �ļ�����·��ǰ׺ */
    "fileMaxSize": 51200000, /* �ϴ���С���ƣ���λB��Ĭ��50MB */
    "fileAllowFiles": [
        ".png", ".jpg", ".jpeg", ".gif", ".bmp",
        ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid",
        ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".cab", ".iso",
        ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md", ".xml"
    ], /* �ϴ��ļ���ʽ��ʾ */

    /* �г�ָ��Ŀ¼�µ�ͼƬ */
    "imageManagerActionName": "listimage", /* ִ��ͼƬ�����action���� */
    "imageManagerListPath": "upload/image", /* ָ��Ҫ�г�ͼƬ��Ŀ¼ */
    "imageManagerListSize": 20, /* ÿ���г��ļ����� */
    "imageManagerUrlPrefix": "/ueditor/net/", /* ͼƬ����·��ǰ׺ */
    "imageManagerInsertAlign": "none", /* �����ͼƬ������ʽ */
    "imageManagerAllowFiles": [".png", ".jpg", ".jpeg", ".gif", ".bmp"], /* �г����ļ����� */

    /* �г�ָ��Ŀ¼�µ��ļ� */
    "fileManagerActionName": "listfile", /* ִ���ļ������action���� */
    "fileManagerListPath": "upload/file", /* ָ��Ҫ�г��ļ���Ŀ¼ */
    "fileManagerUrlPrefix": "/ueditor/net/", /* �ļ�����·��ǰ׺ */
    "fileManagerListSize": 20, /* ÿ���г��ļ����� */
    "fileManagerAllowFiles": [
        ".png", ".jpg", ".jpeg", ".gif", ".bmp",
        ".flv", ".swf", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg",
        ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid",
        ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".cab", ".iso",
        ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md", ".xml"
    ] /* �г����ļ����� */

}