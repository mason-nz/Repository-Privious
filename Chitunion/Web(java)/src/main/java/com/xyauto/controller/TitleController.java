package com.xyauto.controller;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.annotation.Resource;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpSession;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.SessionAttribute;

import com.xyauto.domain.CarTitleListQuery;
import com.xyauto.domain.DictInfo;
import com.xyauto.domain.IpTitleInfo;
import com.xyauto.domain.MediaInfoQuery;
import com.xyauto.domain.MediaTitleListQuery;
import com.xyauto.domain.QueryByTitleNamePojo;
import com.xyauto.domain.TitleBasicInfo;
import com.xyauto.domain.UserInfo;
import com.xyauto.mapper.DictInfoMapper;
import com.xyauto.service.TitleService;
import com.xyauto.util.AuthoUtil;
import com.xyauto.util.CacheUtil;
import com.xyauto.util.SysConstants;
import com.xyauto.util.page.Pagination;

import lombok.extern.slf4j.Slf4j;

/**
 * 
 * @author zhangmg
 * @version 1.0
 */
@Controller
@RequestMapping("title")
@Slf4j
public class TitleController {
	@Resource
	private TitleService titleService;
	@Resource
	private DictInfoMapper dictInfoMapper;

	@RequestMapping("/")
	public String toTitle() {
		return "toTitle";
	}

	/**
	 * 进入媒体对应的标签维护，参数为媒体类型mediaTypeId，媒体mediaId
	 * 
	 * @return
	 * @throws Exception
	 */
	@RequestMapping("/setTitleForMedia/{mediaType}/{mediaId}/{setOrUpdate}")
	public String toAllTitle(@PathVariable Integer mediaType, @PathVariable Integer mediaId,
			@PathVariable Integer setOrUpdate, Model model, HttpServletRequest request) throws Exception {
		Map<String, Object> authoMap = AuthoUtil.getAuthoMap(request, "SYS001BUT6000101,SYS001BUT6000102");
		if (true != (Boolean) authoMap.get("SYS001BUT6000101") && true != (Boolean) authoMap.get("SYS001BUT6000102")) {
			return "hello";
		}
		HttpSession session = request.getSession();
		session.setAttribute("mediaTypeId", mediaType);
		session.setAttribute("mediaId", mediaId);
		session.setAttribute("setOrUpdate", setOrUpdate);
		MediaInfoQuery mediaInfoQuery = titleService.getMediaTypeNameAndMediaNum(mediaType, mediaId);
		model.addAttribute("mediaInfoQuery", mediaInfoQuery);
		model.addAttribute("authoMap", authoMap);
		log.info("operation on titleofmedia");
		return "TitleKeep";
	}

	/**
	 * 进入品牌车系对应的标签维护，参数为媒体类型品牌ID，车系ID
	 * 
	 * @return
	 * @throws Exception
	 */
	@RequestMapping("/setTitleForBrand/{brandID}/{serialID}/{setOrUpdate}")
	public String toBrandTitle(@PathVariable Integer setOrUpdate, @PathVariable Integer brandID,
			@PathVariable Integer serialID, HttpServletRequest request, Model model) throws Exception {
		Map<String, Object> authoMap = AuthoUtil.getAuthoMap(request, "SYS001BUT6000201,SYS001BUT6000202");
		if (true != (Boolean) authoMap.get("SYS001BUT6000201") && true != (Boolean) authoMap.get("SYS001BUT6000202")) {
			return "hello";
		}
		String aboutName = titleService.getNameByBrandIdAndSerialId(brandID, serialID);
		HttpSession session = request.getSession();
		model.addAttribute("aboutName", aboutName);
		session.setAttribute("brandID", brandID);
		session.setAttribute("serialID", serialID);
		session.setAttribute("setOrUpdate", setOrUpdate);
		log.info("operation on titleofCarBrand");

		return "TitleKeepBrand";
	}

	/**
	 * 查询所有标签
	 * 
	 * @return
	 * @throws IOException
	 */
	@RequestMapping("/allTitle")
	public @ResponseBody List<TitleBasicInfo> allTitle() throws IOException {
		List<TitleBasicInfo> list = titleService.selectAllTitle();
		return list;
	}

	/**
	 * 更新媒体标签
	 * 
	 * @param selectedId
	 * @param request
	 * @return
	 */
	@RequestMapping("/updateMediaTitle")
	public @ResponseBody Map<String, Object> updateMediaTitle(@SessionAttribute("mediaTypeId") Integer mediaType,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo user, @SessionAttribute("mediaId") Integer mediaId,
			@SessionAttribute("setOrUpdate") Integer setOrUpdate, String selectedId, HttpServletRequest request) {
		ArrayList<Integer> list = new ArrayList<>();
		if (!"".equals(selectedId)) {
			String[] idStrs = selectedId.split("-");
			for (int i = 0; i < idStrs.length; i++) {
				list.add(Integer.parseInt(idStrs[i]));
			}
		}
		Integer userId = null;
		if (null != user) {
			userId = user.getUserID();
		}
		titleService.updateMediaTitle(mediaType, mediaId, list, userId);
		HashMap<String, Object> back = new HashMap<>();
		back.put("status", "ok");
		back.put("mediaTypeId", mediaType);
		back.put("setOrUpdate", setOrUpdate);
		return back;
	}

	/**
	 * 更新品牌车系的标签
	 * 
	 * @return
	 */
	@RequestMapping("/updateCarBrandTitle")
	public @ResponseBody Map<String, Object> updateCarBrandTitle(String selectedId, HttpServletRequest request,
			@SessionAttribute("brandID") Integer brandID, @SessionAttribute("serialID") Integer serialID,
			@SessionAttribute(SysConstants.SEESION_USER) UserInfo user,
			@SessionAttribute("setOrUpdate") Integer setOrUpdate) {
		if ("".equals(selectedId)) {
			titleService.deleteAllTitleAboutCarBrand(brandID, serialID);
		} else {
			String[] idStrs = selectedId.split("-");

			ArrayList<Integer> list = new ArrayList<>();
			for (int i = 0; i < idStrs.length; i++) {
				list.add(Integer.parseInt(idStrs[i]));
			}
			Integer userId = null;
			if (null != user) {
				userId = user.getUserID();
			}
			titleService.updateCarBrandTitle(brandID, serialID, list, userId);
		}
		HashMap<String, Object> back = new HashMap<>();
		back.put("status", "ok");
		back.put("setOrUpdate", setOrUpdate);
		return back;
	}

	/**
	 * 根据媒体id和类型查询所属标签集合
	 * 
	 * @param record
	 * @return
	 */
	@RequestMapping("getTitlesByMediaIdAndMediaType")
	public @ResponseBody Map<String, Object> getTitlesByMediaIdAndMediaType(HttpServletRequest request) {
		Map<String, Object> back = new HashMap<String, Object>();
		HttpSession session = request.getSession();
		Integer mediaType = (Integer) session.getAttribute("mediaTypeId");
		Integer mediaId = (Integer) session.getAttribute("mediaId");
		try {
			List<TitleBasicInfo> titles = titleService.getTitlesByMediaIdAndMediaType(mediaType, mediaId);
			back.put("titles", titles);
			return back;
		} catch (Exception e) {
			return back;
		}

	}

	@RequestMapping("getTitleByBrandIdAndSerialID")
	public @ResponseBody Map<String, Object> getTitleByBrandIdAndSerialID(HttpServletRequest request) {
		HashMap<String, Object> back = new HashMap<String, Object>();
		HttpSession session = request.getSession();
		Integer brandID = (Integer) session.getAttribute("brandID");
		Integer serialID = (Integer) session.getAttribute("serialID");
		List<TitleBasicInfo> titles = titleService.getTitlesByBrandIdAndSerialId(brandID, serialID);
		back.put("titles", titles);
		return back;
	}

	@RequestMapping("/testCach")
	public void testCach() {
		Map<Integer, DictInfo> dictMap = CacheUtil.getDictMap(dictInfoMapper);
		DictInfo dictInfo = dictMap.get(14001);
		Integer dictId = dictInfo.getDictId();
		log.debug("" + dictId);
	}

	/**
	 * 未打标签的媒体
	 * 
	 * @param pageNo
	 * @param mediaTypeId
	 * @param model
	 * @return
	 * @throws Exception
	 * @throws Exception
	 * @throws InterruptedException
	 */
	@RequestMapping("mediaNoTitleList")
	public String mediaNoTitleList(HttpServletRequest request, Integer pageNo, Integer mediaTypeId, Model model)
			throws Exception {
		Map<String, Object> authoMap = AuthoUtil.getAuthoMap(request,
				"SYS001MOD60001,SYS001BUT6000101,SYS001BUT6000102");
		if (true != (Boolean) authoMap.get("SYS001MOD60001")) {
			return "hello";
		}
		// 用来判断列表查询页面是否出现设置标签按钮
		model.addAttribute("authoMap", authoMap);
		if (null == mediaTypeId) {
			mediaTypeId = 14001;
		}
		Integer pageRows = SysConstants.TITLE_PAGEROWS;
		Map<String, Integer> map = new HashMap<String, Integer>();
		map.put("curPage", Pagination.cpn(pageNo));
		map.put("pageRows", pageRows);
		map.put("count", -1);
		List<MediaTitleListQuery> list = titleService.pagination(mediaTypeId, map);
		Integer count = map.get("count");
		Pagination pagination = new Pagination(Pagination.cpn(pageNo), pageRows, count, list);
		StringBuilder params = new StringBuilder();
		params.append("mediaTypeId=").append(mediaTypeId);
		pagination.pageView("/title/mediaNoTitleList", params.toString());
		model.addAttribute("mediaTypeId", mediaTypeId);
		model.addAttribute("pagination", pagination);
		return "mediaWithOutTitle";
	}

	/**
	 * 已打标签
	 * 
	 * @param pageNo
	 * @param createTime
	 * @param username
	 * @param titleName
	 * @param mediaTypeId
	 * @param model
	 * @return
	 * @throws Exception
	 */
	@RequestMapping("mediaWithTitleList")
	public String mediaWithTitleList(HttpServletRequest request, Integer pageNo, String createTime, String createTime1,
			String username, String titleName, Integer mediaTypeId, Model model) throws Exception {
		Map<String, Object> authoMap = AuthoUtil.getAuthoMap(request,
				"SYS001MOD60001,SYS001BUT6000101,SYS001BUT6000102");
		if (true != (Boolean) authoMap.get("SYS001MOD60001")) {
			return "hello";
		}
		// 用来判断列表查询页面是否出现设置标签按钮
		model.addAttribute("authoMap", authoMap);

		int pageRows = SysConstants.TITLE_PAGEROWS;
		if (null == mediaTypeId) {
			mediaTypeId = 14001;
		}
		StringBuilder params = new StringBuilder();
		params.append("mediaTypeId=").append(mediaTypeId);
		Map<String, Object> map = new HashMap<String, Object>();
		map.put("curPage", Pagination.cpn(pageNo));
		map.put("pageRows", pageRows);
		map.put("count", -1);
		StringBuilder sb = null;
		if (mediaTypeId != null) {
			if (14001 == mediaTypeId) {
				sb = new StringBuilder(
						"select * yanfafrom (SELECT   HeadIconURL, wx.MediaID, wx.Number, wx.Name,(SELECT ' ' + tb.Name FROM  Title_Media_Mapping tm LEFT JOIN TitleBasicInfo tb ON tm.TitleID = tb.TitleID"
								+ " WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14001 FOR XML PATH('')) AS title,"
								+ "(SELECT TOP 1 ui.UserName FROM Title_Media_Mapping tm LEFT JOIN UserInfo ui ON tm.CreateUserID = ui.UserID"
								+ " WHERE  tm.MediaID = wx.MediaID AND tm.MediaType = 14001 ORDER BY tm.CreateTime DESC) AS CreateUserName,"
								+ "(SELECT TOP 1 tm.CreateTime FROM Title_Media_Mapping tm LEFT JOIN UserInfo ui ON tm.CreateUserID = ui.UserID"
								+ " WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14001 ORDER BY tm.CreateTime DESC) AS CreateTime,'14001' AS MediaTypeId"
								+ " FROM Media_Weixin wx WHERE wx.MediaID IN (SELECT MAX(mw.MediaID) FROM Media_Weixin mw GROUP BY mw.Number)) as A where title is not null");

			}
			if (14002 == mediaTypeId) {
				sb = new StringBuilder(
						"select * yanfafrom (SELECT   HeadIconURL, app.MediaID,(SELECT SUBSTRING ((SELECT ', '+ A.DictName FROM (SELECT DISTINCT dif.DictName FROM DictInfo dif, Media_PCAPP"
								+ " WHERE dif.DictId IN (SELECT * FROM f_split(app.Terminal, ','))) AS A FOR XML PATH('')), 2, 20)) AS Number, app.Name,(SELECT ' ' + tb.Name FROM Title_Media_Mapping tm LEFT JOIN"
								+ " TitleBasicInfo tb ON tm.TitleID = tb.TitleID WHERE   tm.MediaID = app.MediaID AND tm.MediaType = 14002 FOR XML PATH('')) AS title,(SELECT TOP 1 ui.UserName FROM Title_Media_Mapping tm LEFT JOIN"
								+ " UserInfo ui ON tm.CreateUserID = ui.UserID WHERE   tm.MediaID = app.MediaID AND tm.MediaType = 14002 ORDER BY tm.CreateTime DESC) AS CreateUserName,(SELECT TOP 1 tm.CreateTime"
								+ " FROM Title_Media_Mapping tm LEFT JOIN UserInfo ui ON tm.CreateUserID = ui.UserID WHERE   tm.MediaID = app.MediaID AND tm.MediaType = 14002 ORDER BY tm.CreateTime DESC) AS CreateTime, '14002' AS MediaTypeId"
								+ " FROM Media_PCAPP app) as A  where title is not null");

			}
			if (14003 == mediaTypeId) {
				sb = new StringBuilder(
						"select * yanfafrom (SELECT HeadIconURL, wx.MediaID, wx.Number, wx.Name,(SELECT ' '+ tb.Name FROM Title_Media_Mapping tm LEFT JOIN TitleBasicInfo tb ON tm.TitleID = tb.TitleID"
								+ " WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14003 FOR XML PATH('')) AS title,(SELECT TOP 1 ui.UserName FROM Title_Media_Mapping tm LEFT JOIN UserInfo ui ON tm.CreateUserID = ui.UserID"
								+ " WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14003 ORDER BY tm.CreateTime DESC) AS CreateUserName,(SELECT TOP 1 tm.CreateTime FROM Title_Media_Mapping tm LEFT JOIN UserInfo ui ON tm.CreateUserID = ui.UserID"
								+ " WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14003 ORDER BY tm.CreateTime DESC) AS CreateTime, '14003' AS MediaTypeId FROM Media_Weibo wx WHERE wx.MediaID IN (SELECT MAX(mw.MediaID) FROM Media_Weibo mw"
								+ " GROUP BY mw.Number)) as A where title is not null");

			}
			if (14004 == mediaTypeId) {
				sb = new StringBuilder(
						"select * yanfafrom (SELECT HeadIconURL, wx.MediaID, wx.Number, wx.Name,(SELECT ' ' + tb.Name FROM Title_Media_Mapping tm LEFT JOIN TitleBasicInfo tb ON tm.TitleID = tb.TitleID"
								+ " WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14004 FOR XML PATH('')) AS title,(SELECT TOP 1 ui.UserName FROM Title_Media_Mapping tm LEFT JOIN UserInfo ui ON tm.CreateUserID = ui.UserID"
								+ " WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14004 ORDER BY tm.CreateTime DESC) AS CreateUserName,(SELECT TOP 1 tm.CreateTime FROM Title_Media_Mapping tm LEFT JOIN UserInfo ui ON tm.CreateUserID = ui.UserID"
								+ " WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14004 ORDER BY tm.CreateTime DESC) AS CreateTime, '14004' AS MediaTypeId FROM Media_Video wx WHERE wx.MediaID IN (SELECT MAX(mw.MediaID)"
								+ " FROM Media_Video mw GROUP BY mw.Number)) as A where title is not null");
			}
			if (14005 == mediaTypeId) {
				sb = new StringBuilder(
						"select * yanfafrom (SELECT HeadIconURL, wx.MediaID, wx.Number, wx.Name,(SELECT ' ' + tb.Name FROM Title_Media_Mapping tm LEFT JOIN TitleBasicInfo tb ON tm.TitleID = tb.TitleID"
								+ " WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14005 FOR XML PATH('')) AS title,(SELECT TOP 1 ui.UserName FROM Title_Media_Mapping tm LEFT JOIN UserInfo ui ON tm.CreateUserID = ui.UserID WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14005"
								+ " ORDER BY tm.CreateTime DESC) AS CreateUserName,(SELECT TOP 1 tm.CreateTime FROM Title_Media_Mapping tm LEFT JOIN UserInfo ui ON tm.CreateUserID = ui.UserID WHERE tm.MediaID = wx.MediaID AND tm.MediaType = 14005"
								+ " ORDER BY tm.CreateTime DESC) AS CreateTime, '14005' AS MediaTypeId FROM Media_Broadcast wx WHERE   wx.MediaID IN (SELECT MAX(mw.MediaID) FROM Media_Broadcast mw GROUP BY mw.Number)) as A where title is not null");
			}

			if (null != username && !"".equals(username)) {
				String replace = username.replace("'", "").trim();
				params.append("&username=").append(replace);
				sb.append(" and CreateUserName like " + "'%" + replace + "%'");
			}
			if (null != titleName && !"".equals(titleName)) {
				String replace = titleName.replace("'", "").trim();
				params.append("&titleName=").append(replace);
				sb.append(" and title like " + "'%" + replace + "%'");
			}
			String format1 = null;
			String format2 = null;
			Date afterDay = null;
			SimpleDateFormat formatDate1 = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
			SimpleDateFormat formatDate = new SimpleDateFormat("yyyy-MM-dd");
			if (null != createTime && !"".equals(createTime)) {
				params.append("&createTime=").append(createTime);
				String replace = createTime.replace("'", "").trim();
				Date date = formatDate.parse(replace);
				format1 = formatDate1.format(date);
				if (null == createTime1 || "".equals(createTime1)) {
					sb.append(" and createTime > " + "'" + format1 + "'");
				} else {
					params.append("&createTime1=").append(createTime1);
					String replace1 = createTime1.replace("'", "").trim();
					Date date2 = formatDate.parse(replace1);
					Long nowValue = date2.getTime();// date的毫秒数
					Long afterDayMinutes = nowValue + 24 * 60 * 60 * 1000;// date加一天的毫秒数
					afterDay = new Date(afterDayMinutes);
					format2 = formatDate1.format(afterDay);
					sb.append(" and CreateTime > '" + format1 + "' and CreateTime < '" + format2 + "'");
				}
			} else {
				if (null != createTime1 && !"".equals(createTime1)) {
					params.append("&createTime1=").append(createTime1);
					String replace1 = createTime1.replace("'", "").trim();
					Date date2 = formatDate.parse(replace1);
					Long nowValue = date2.getTime();// date的毫秒数
					Long afterDayMinutes = nowValue + 24 * 60 * 60 * 1000;// date加一天的毫秒数
					afterDay = new Date(afterDayMinutes);
					format2 = formatDate1.format(afterDay);
					sb.append(" and CreateTime < '" + format2 + "'");
				}
			}
		}
		if (null != mediaTypeId && 10000 != mediaTypeId) {
			sb.append(" and MediaTypeId=" + mediaTypeId);
		}
		if (null != sb) {
			map.put("append", sb.toString());
		}
		List<MediaTitleListQuery> list = titleService.MediaWithTitlePagination(mediaTypeId, map);
		Integer count = (Integer) map.get("count");
		Pagination pagination = new Pagination(Pagination.cpn(pageNo), pageRows, count, list);
		pagination.pageView("/title/mediaWithTitleList", params.toString());
		model.addAttribute("mediaTypeId", mediaTypeId);
		model.addAttribute("pagination", pagination);
		return "mediaWithTitle";
	}

	@RequestMapping("carBrandWithTitleList")
	public String carBrandWithTitleList(HttpServletRequest request, Integer pageNo, String titleName,
			String createTime1, String createUserName, String lastUpdateTime, Integer BrandID, Model model)
			throws Exception {
		Map<String, Object> authoMap = AuthoUtil.getAuthoMap(request,
				"SYS001MOD60002,SYS001BUT6000201,SYS001BUT6000202");
		if (true != (Boolean) authoMap.get("SYS001MOD60002")) {
			return "hello";
		}
		// 页面判断是否显示标签维护
		model.addAttribute("authoMap", authoMap);
		int pageRows = SysConstants.TITLE_PAGEROWS;
		StringBuilder params = new StringBuilder();
		Map<String, Object> map = new HashMap<String, Object>();
		map.put("curPage", Pagination.cpn(pageNo));
		map.put("pageRows", pageRows);
		map.put("count", -1);
		if (null != BrandID) {
			params.append("BrandID=").append(BrandID);
		} else {
			params.append("BrandID=").append("0");
		}
		StringBuilder sb = new StringBuilder("select * yanfafrom brandtitle_view where 1=1");
		if (null != titleName && !"".equals(titleName)) {
			String replace = titleName.replace("'", "").trim();
			params.append("&titleName=").append(replace);
			sb.append(" and titles like " + "'%" + replace + "%'");
		}
		if (null != createUserName && !"".equals(createUserName)) {
			String replace = createUserName.replace("'", "").trim();
			params.append("&createUserName=").append(replace);
			sb.append(" and CreateUserName like " + "'%" + replace + "%'");
		}
		SimpleDateFormat formatDate = new SimpleDateFormat("yyyy-MM-dd");
		SimpleDateFormat formatDate1 = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		String format2 = null;
		Date afterDay = null;
		if (null != lastUpdateTime && !"".equals(lastUpdateTime)) {
			String replace = lastUpdateTime.replace("'", "").trim();
			params.append("&lastUpdateTime=").append(replace);
			Date date = formatDate.parse(replace);
			String format1 = formatDate1.format(date);
			if (null == createTime1 || "".equals(createTime1)) {
				sb.append(" and ModifiedTime > " + "'" + format1 + "'");
			} else {
				params.append("&createTime1=").append(createTime1);
				String replace1 = createTime1.replace("'", "").trim();
				Date date2 = formatDate.parse(replace1);
				Long nowValue = date2.getTime();// date的毫秒数
				Long afterDayMinutes = nowValue + 24 * 60 * 60 * 1000;// date加一天的毫秒数
				afterDay = new Date(afterDayMinutes);
				format2 = formatDate1.format(afterDay);
				sb.append(" and ModifiedTime > '" + format1 + "' and ModifiedTime < '" + format2 + "'");
			}
		} else {
			if (null != createTime1 && !"".equals(createTime1)) {
				params.append("&createTime1=").append(createTime1);
				String replace1 = createTime1.replace("'", "").trim();
				Date date2 = formatDate.parse(replace1);
				Long nowValue = date2.getTime();// date的毫秒数
				Long afterDayMinutes = nowValue + 24 * 60 * 60 * 1000;// date加一天的毫秒数
				afterDay = new Date(afterDayMinutes);
				format2 = formatDate1.format(afterDay);
				sb.append(" and ModifiedTime < '" + format2 + "'");
			}
		}
		if (null != BrandID && (0 != BrandID)) {
			sb.append(" and BrandID=" + BrandID);
		}
		map.put("append", sb.toString());
		List<CarTitleListQuery> list = titleService.carBrandTitleList(map);
		List<CarTitleListQuery> brands = titleService.getBrandsWithTitle();
		model.addAttribute("brands", brands);
		Integer count = (Integer) map.get("count");
		Pagination pagination = new Pagination(Pagination.cpn(pageNo), pageRows, count, list);
		pagination.pageView("/title/carBrandWithTitleList", params.toString());
		model.addAttribute("pagination", pagination);
		return "carBrandWithTitleList";
	}

	// ajax回显下拉选
	/*
	 * @RequestMapping("/getAllBrandWithTitle") public @ResponseBody Map<String,
	 * Object> getAllBrandWithTitle() { Map<String, Object> back = new
	 * HashMap<String, Object>(); List<CarTitleListQuery> brands =
	 * titleService.getBrandsWithTitle(); back.put("brands", brands); return
	 * back; }
	 */
	/**
	 * 未打标签的品牌车系列表，入口方法
	 * 
	 * @param pageNo
	 * @param BrandID
	 * @param model
	 * @return
	 * @throws Exception
	 */
	@RequestMapping("getAllBrandWithOutTitleList")
	public String getAllBrandWithOutTitleList(HttpServletRequest request, Integer pageNo, Integer BrandID, Model model)
			throws Exception {
		Map<String, Object> authoMap = AuthoUtil.getAuthoMap(request,
				"SYS001MOD60002,SYS001BUT6000201,SYS001BUT6000202");
		if (true != (Boolean) authoMap.get("SYS001MOD60002")) {
			return "hello";
		}
		// 页面判断是否显示标签维护
		model.addAttribute("authoMap", authoMap);
		int pageRows = SysConstants.TITLE_PAGEROWS;
		StringBuilder params = new StringBuilder();
		Map<String, Object> map = new HashMap<String, Object>();
		map.put("curPage", Pagination.cpn(pageNo));
		map.put("pageRows", pageRows);
		map.put("count", -1);
		if (null == BrandID) {
			BrandID = 0;
			params.append("BrandID=").append("0");
		} else {
			params.append("BrandID=").append(BrandID);
		}
		StringBuilder sb = new StringBuilder("select * yanfafrom brandWithOutTitle where 1=1");

		if (null != BrandID && 0 != BrandID) {
			params.append("&BrandID=").append(BrandID);
			sb.append(" and BrandID =" + BrandID);

		}
		List<CarTitleListQuery> brands = titleService.getJustBrandNoTitle();
		model.addAttribute("brands", brands);
		// model.addAttribute("brandID", BrandID); // 这个id用于页面下拉选回选
		map.put("append", sb.toString());
		List<CarTitleListQuery> list = titleService.getBrandsWithOutTitles(map);
		Integer count = (Integer) map.get("count");
		Pagination pagination = new Pagination(Pagination.cpn(pageNo), pageRows, count, list);
		pagination.pageView("/title/getAllBrandWithOutTitleList", params.toString());
		model.addAttribute("pagination", pagination);
		return "carBrandWithOutTitleList";
	}
	/**
	 * 未打标签下拉选
	 * 
	 * @return
	 *//*
		 * @RequestMapping("getAllJustBrandNoTitles") public @ResponseBody
		 * Map<String, Object> getAllJustBrandNoTitles() { HashMap<String,
		 * Object> map = new HashMap<>(); List<CarTitleListQuery> brands =
		 * titleService.getJustBrandNoTitle(); map.put("brands", brands); return
		 * map; }
		 */

	/**
	 * 标签、用户查询
	 * 
	 * @param request
	 * @param pageNo
	 * @param queryType
	 * @param titleName
	 * @param model
	 * @return
	 * @throws Exception
	 */

	/*
	 * public @ResponseBody QueryByTitleNamePojo queryByTypeAndTitleName(
	 * 
	 * @RequestBody QueryByTitleNamePojo queryByTitleNamePojo,
	 * HttpServletRequest request) {
	 */
	@RequestMapping(value = "queryByTypeAndTitleName")
	public @ResponseBody QueryByTitleNamePojo queryByTypeAndTitleName(
			@RequestBody QueryByTitleNamePojo queryByTitleNamePojo, HttpServletRequest request) {
		if (null == queryByTitleNamePojo.getQueryTypeId()) {
			queryByTitleNamePojo.setQueryTypeId(0);
		}
		/*
		 * Integer curPage = Integer.valueOf(request.getParameter("curPage"));
		 * queryByTitleNamePojo.setCurPage(curPage);
		 * queryByTitleNamePojo.setQueryTypeId(Integer.valueOf(request.
		 * getParameter("queryTypeId")));
		 * queryByTitleNamePojo.setTitleName(request.getParameter("titleName"));
		 */
		List<String> list = null;
		List<MediaTitleListQuery> mediaList = null;
		StringBuilder sb = new StringBuilder("");
		Map<String, Object> map = new HashMap<String, Object>();
		map.put("curPage", queryByTitleNamePojo.getCurPage());
		map.put("pageRows", SysConstants.TITLE_PAGEROWS);
		map.put("count", -1);
		if (null != queryByTitleNamePojo.getTitleName() && !"".equals(queryByTitleNamePojo.getTitleName())) {
			queryByTitleNamePojo.setTitleName(queryByTitleNamePojo.getTitleName().replace("'", "").trim());
		}
		if (queryByTitleNamePojo.getQueryTypeId() == 0) {
			sb.append(
					"select * yanfafrom (select cb.Name as BrandName ,null as SerialName  from Car_Brand cb left join Title_Car_Mapping tcm on cb.BrandID = tcm.CarBrandID where tcm.TitleID = (select tb.TitleID  from TitleBasicInfo tb where tb.Name = '"
							+ queryByTitleNamePojo.getTitleName()
							+ "') and tcm.CSID is null union all select cb.Name BrandName, cs.Name SerialName   from Car_Serial cs left join Title_Car_Mapping tcm on cs.SerialID= tcm.CSID left join Car_Brand cb on tcm.CarBrandID = cb.BrandID where tcm.TitleID = (select tb.TitleID  from TitleBasicInfo tb where tb.Name = '"
							+ queryByTitleNamePojo.getTitleName() + "'))  as A");
			map.put("append", sb.toString());
			list = titleService.queryCarBrandByTypeAndTitleName(map);
		}
		if (queryByTitleNamePojo.getQueryTypeId() == 1) {
			sb.append(
					"select tm.MediaID,tm.MediaType  yanfafrom Title_Media_Mapping tm where tm.TitleID = (select tb.TitleID  from TitleBasicInfo tb where tb.Name = '"
							+ queryByTitleNamePojo.getTitleName() + "')");
			map.put("append", sb.toString());
			mediaList = titleService.queryMediaByTitleName(map);
		}
		queryByTitleNamePojo.setMediaQueryResult(mediaList);
		queryByTitleNamePojo.setResultTitleName(list);
		queryByTitleNamePojo.setCount((Integer) map.get("count"));
		queryByTitleNamePojo.setPageRows((Integer) map.get("pageRows"));
		return queryByTitleNamePojo;
	}

	@RequestMapping("toQueryTitle")
	public String toQueryByTitleName(HttpServletRequest request) {
		try {
			Map<String, Object> authoMap = AuthoUtil.getAuthoMap(request, "SYS001MOD60003");
			if (true != (Boolean) authoMap.get("SYS001MOD60003")) {
				return "hello";
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return "queryTitle";
	}

	/**
	 * 当鼠标移到已经选定的标签上，显示该标签的所有关系
	 * 
	 * @param TitleId
	 * @return
	 */
	@RequestMapping("getRelationByTitleId")
	public @ResponseBody List<IpTitleInfo> getRelationByTitleId(Integer TitleId) {
		List<IpTitleInfo> relationList = titleService.getRelationByTitleID(TitleId);
		return relationList;
	}

}
