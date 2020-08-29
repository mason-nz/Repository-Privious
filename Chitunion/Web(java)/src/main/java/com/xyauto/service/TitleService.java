package com.xyauto.service;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Map;

import javax.annotation.Resource;

import org.springframework.stereotype.Service;

import com.xyauto.domain.CarTitleListQuery;
import com.xyauto.domain.DictInfo;
import com.xyauto.domain.IpTitleInfo;
import com.xyauto.domain.MediaInfoQuery;
import com.xyauto.domain.MediaTitleListQuery;
import com.xyauto.domain.TitleBasicInfo;
import com.xyauto.domain.TitleCarMapping;
import com.xyauto.domain.TitleMediaMapping;
import com.xyauto.mapper.DictInfoMapper;
import com.xyauto.mapper.IpTitleInfoMapper;
import com.xyauto.mapper.TitleBasicInfoMapper;
import com.xyauto.mapper.TitleMediaMappingMapper;
import com.xyauto.util.CacheUtil;

@Service
public class TitleService {
	@Resource
	private DictInfoMapper dictInfoMapper;
	@Resource
	private TitleBasicInfoMapper titlebasicinfoMapper;
	@Resource
	private TitleMediaMappingMapper titleMediaMappingMapper;
	@Resource
	private IpTitleInfoMapper iptitleinfoMapper;

	public List<TitleBasicInfo> selectAllTitle() {
		List<TitleBasicInfo> selectAllTitle = titlebasicinfoMapper.selectAllTitle();
		for (TitleBasicInfo titleBasicInfo : selectAllTitle) {
			if (1 == titleBasicInfo.getType()) {
				Integer getpId = titleBasicInfo.getpId();
				Integer titleid = titleBasicInfo.getTitleid();
				titleBasicInfo.setTitleid(getpId * titleid);
			}
			if (2 == titleBasicInfo.getType()) {
				Integer getpId = titleBasicInfo.getpId();
				Integer rp = titleBasicInfo.getRp();
				titleBasicInfo.setPipId(getpId * rp);
			}

		}
		return selectAllTitle;
	}

	public List<Integer> getTitleIdByMediaIdAndMediaType(Integer mediaType, Integer mediaId) {
		return titleMediaMappingMapper.getTitleIdByMediaIdAndMediaType(mediaType, mediaId);
	}

	public List<TitleBasicInfo> test(Map<String, Integer> map) {
		return titlebasicinfoMapper.test(map);
	}

	public List<TitleBasicInfo> getTitlesByMediaIdAndMediaType(Integer mediaType, Integer mediaId) {
		return titleMediaMappingMapper.getTitlesByMediaIdAndMediaType(mediaType, mediaId);
	}

	/**
	 * 未打标签媒体分页
	 * 
	 * @param mediaTypeId
	 * @param map
	 * @return
	 */
	public List<MediaTitleListQuery> pagination(Integer mediaTypeId, Map<String, Integer> map) {
		List<MediaTitleListQuery> mediaQuerys = null;
		if (14001 == mediaTypeId) {
			mediaQuerys = titleMediaMappingMapper.getWeiXinWithOutTitle(map);
		}
		if (14002 == mediaTypeId) {
			mediaQuerys = titleMediaMappingMapper.getAppWithOutTitle(map);
		}
		if (14003 == mediaTypeId) {
			mediaQuerys = titleMediaMappingMapper.getWeiBoWithOutTitle(map);
		}
		if (14004 == mediaTypeId) {
			mediaQuerys = titleMediaMappingMapper.getVideoWithOutTitle(map);
		}
		if (14005 == mediaTypeId) {
			mediaQuerys = titleMediaMappingMapper.getBroadCastWithOutTitle(map);
		}
		return mediaQuerys;
	}

	public List<MediaTitleListQuery> MediaWithTitlePagination(Integer mediaTypeId, Map<String, Object> map) {
		List<MediaTitleListQuery> mediaQuerys = titleMediaMappingMapper.getAllMediaWithTitleList(map);
		for (MediaTitleListQuery mediaTitleListQuery : mediaQuerys) {
			if (null != mediaTitleListQuery.getMediaTypeId() && 14001 == mediaTitleListQuery.getMediaTypeId()) {
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14001).getDictName());
			}
			if (null != mediaTitleListQuery.getMediaTypeId() && 14002 == mediaTitleListQuery.getMediaTypeId()) {
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14002).getDictName());
			}
			if (null != mediaTitleListQuery.getMediaTypeId() && 14003 == mediaTitleListQuery.getMediaTypeId()) {
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14003).getDictName());
			}
			if (null != mediaTitleListQuery.getMediaTypeId() && 14004 == mediaTitleListQuery.getMediaTypeId()) {
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14004).getDictName());
			}
			if (null != mediaTitleListQuery.getMediaTypeId() && 14005 == mediaTitleListQuery.getMediaTypeId()) {
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14005).getDictName());
			}
			SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
			Date mediaCreateTime = mediaTitleListQuery.getMediaCreateTime();
			String format = sdf.format(mediaCreateTime);
			mediaTitleListQuery.setShowTime(format);
		}
		return mediaQuerys;
	}

	public List<CarTitleListQuery> carBrandTitleList(Map<String, Object> map) {
		List<CarTitleListQuery> carBrands = titleMediaMappingMapper.getAllCarBrandListWithTitle(map);
		for (CarTitleListQuery carTitleListQuery : carBrands) {
			SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
			Date mediaCreateTime = carTitleListQuery.getModifiedTime();
			String format = sdf.format(mediaCreateTime);
			carTitleListQuery.setShowTime(format);
		}
		return carBrands;
	}

	public List<CarTitleListQuery> getBrandsWithTitle() {
		return titleMediaMappingMapper.getBrandsWithTitle();
	}

	public List<CarTitleListQuery> getBrandsWithOutTitles(Map<String, Object> map) {
		return titleMediaMappingMapper.getAllBrandsWithOutTitle(map);
	}

	public List<CarTitleListQuery> getJustBrandNoTitle() {
		return titleMediaMappingMapper.getJustBrandWithOutTitle();
	}

	public List<String> queryCarBrandByTypeAndTitleName(Map<String, Object> map) {
		ArrayList<String> list = new ArrayList<>();
		List<CarTitleListQuery> list1 = titleMediaMappingMapper.queryCarBrandByTitleName(map);
		for (CarTitleListQuery carTitleListQuery : list1) {
			String showName = carTitleListQuery.getShowName();
			list.add(showName);
		}
		return list;
	}

	public List<MediaTitleListQuery> queryMediaByTitleName(Map<String, Object> map) {
		List<TitleMediaMapping> list1 = titleMediaMappingMapper.queryMediaByTitleName(map);
		ArrayList<MediaTitleListQuery> list = new ArrayList<MediaTitleListQuery>();
		for (TitleMediaMapping titleMediaMapping : list1) {
			if (titleMediaMapping.getMediaType() == 14001) {
				MediaTitleListQuery mediaTitleListQuery = titleMediaMappingMapper
						.getWeiXinMediaNameByMediaId(titleMediaMapping.getMediaId());
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14001).getDictName());
				list.add(mediaTitleListQuery);
			}
			if (titleMediaMapping.getMediaType() == 14002) {
				MediaTitleListQuery mediaTitleListQuery = titleMediaMappingMapper
						.getPCAPPMediaNameByMediaId(titleMediaMapping.getMediaId());
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14002).getDictName());
				list.add(mediaTitleListQuery);
			}
			if (titleMediaMapping.getMediaType() == 14003) {
				MediaTitleListQuery mediaTitleListQuery = titleMediaMappingMapper
						.getWeiBoMediaNameByMediaId(titleMediaMapping.getMediaId());
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14003).getDictName());
				list.add(mediaTitleListQuery);
			}
			if (titleMediaMapping.getMediaType() == 14004) {
				MediaTitleListQuery mediaTitleListQuery = titleMediaMappingMapper
						.getVideoMediaNameByMediaId(titleMediaMapping.getMediaId());
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14004).getDictName());
				list.add(mediaTitleListQuery);
			}
			if (titleMediaMapping.getMediaType() == 14005) {
				MediaTitleListQuery mediaTitleListQuery = titleMediaMappingMapper
						.getBroadCastMediaNameByMediaId(titleMediaMapping.getMediaId());
				mediaTitleListQuery.setMediaTypeName(CacheUtil.getDictMap(dictInfoMapper).get(14005).getDictName());
				list.add(mediaTitleListQuery);
			}

		}
		return list;
	}

	public List<TitleBasicInfo> getTitlesByBrandIdAndSerialId(Integer brandID, Integer serialID) {
		return titleMediaMappingMapper.getTitlesByBrandIdAndSerialId(brandID, serialID);
	}

	public void updateCarBrandTitle(Integer brandID, Integer serialID, ArrayList<Integer> list, Integer userId) {
		Date date = new Date();
		deleteAllTitleAboutCarBrand(brandID, serialID);
		if (-1 == serialID) {
			serialID = null;
		}
		for (int i = 0; i < list.size(); i++) {
			TitleCarMapping record = new TitleCarMapping();// record的初始化在循环外面，导致返回自增主键被赋给record，后面的record插不进去（主键重复）
			record.setCarBrandId(brandID);
			record.setCSID(serialID);
			record.setTitleId(list.get(i));
			record.setCreateUserId(userId);
			record.setCreateTime(date);
			record.setStatus(0);
			titleMediaMappingMapper.addTitleToCarBrandByBrandIdAndSerialId(record);
		}
	}

	public void deleteAllTitleAboutCarBrand(Integer brandID, Integer serialID) {
		if (-1 == serialID) {
			serialID = null;
		}
		titleMediaMappingMapper.deleteAllTitleAboutCarBrand(brandID, serialID);
	}

	/**
	 * 更新媒体标签
	 * 
	 * @param mediaType
	 * @param mediaId
	 * @param list
	 * @param userId
	 */
	public void updateMediaTitle(Integer mediaType, Integer mediaId, ArrayList<Integer> list, Integer userId) {
		List<Integer> mediaIds = new ArrayList<Integer>();
		mediaIds.add(mediaId);
		switch (mediaType) {
		case 14001:
			mediaIds = titleMediaMappingMapper.getOtherMediaIdSameWithWeiXinNumberByMediaId(mediaId);
			break;
		case 14003:
			mediaIds = titleMediaMappingMapper.getOtherMediaIdSameWithWeiboNumberByMediaId(mediaId);
			break;
		case 14004:
			mediaIds = titleMediaMappingMapper.getOtherMediaIdSameWithVideoNumberByMediaId(mediaId);
			break;
		case 14005:
			mediaIds = titleMediaMappingMapper.getOtherMediaIdSameWithBroadcastNumberByMediaId(mediaId);
			break;
		default:
			break;
		}
		Date date = new Date();
		for (Integer mediaId1 : mediaIds) {
			titleMediaMappingMapper.delTitleOfMedia(mediaType, mediaId1);
		}
		if (null != list) {
			for (Integer mediaId1 : mediaIds) {
				for (int i = 0; i < list.size(); i++) {
					TitleMediaMapping record = new TitleMediaMapping();
					record.setCreateTime(date);
					record.setCreateUserId(userId);
					record.setMediaId(mediaId1);
					record.setMediaType(mediaType);
					record.setTitleId(list.get(i));
					record.setStatus(0);
					titleMediaMappingMapper.insertSelective(record);
				}

			}
		}

	}

	public List<IpTitleInfo> getRelationByTitleID(Integer titleId) {
		List<IpTitleInfo> relationList = titlebasicinfoMapper.getRelationByTitleID(titleId);
		for (IpTitleInfo ipTitleInfo : relationList) {
			Integer relation = ipTitleInfo.getRelation();
			if (null != relation) {
				DictInfo dictInfo = CacheUtil.getDictMap(dictInfoMapper).get(relation);
				ipTitleInfo.setRelationName((dictInfo.getDictName()));
			}
		}
		return relationList;
	}

	public MediaInfoQuery getMediaTypeNameAndMediaNum(Integer mediaType, Integer mediaId) {
		DictInfo dictInfo = CacheUtil.getDictMap(dictInfoMapper).get(mediaType);
		MediaInfoQuery mediaInfoQuery = null;
		if (14001 == mediaType) {
			mediaInfoQuery = titlebasicinfoMapper.selectWxInfoById(mediaId);
		}
		if (14002 == mediaType) {
			mediaInfoQuery = titlebasicinfoMapper.selectAppInfoById(mediaId);
		}
		if (14003 == mediaType) {
			mediaInfoQuery = titlebasicinfoMapper.selectWbInfoById(mediaId);
		}
		if (14004 == mediaType) {
			mediaInfoQuery = titlebasicinfoMapper.selectVideoInfoById(mediaId);
		}
		if (14005 == mediaType) {
			mediaInfoQuery = titlebasicinfoMapper.selectBroadCastInfoById(mediaId);
		}
		if (null != mediaInfoQuery) {
			mediaInfoQuery.setMediaTypeName(dictInfo.getDictName());
		}
		return mediaInfoQuery;
	}

	public String getNameByBrandIdAndSerialId(Integer brandID, Integer serialID) {
		String name = null;
		if (-1 != serialID) {
			CarTitleListQuery carTitleListQuery = titlebasicinfoMapper.selectSerialName(brandID, serialID);
			name = carTitleListQuery.getBrandName() + "-" + carTitleListQuery.getSerialName();
		} else {
			name = titlebasicinfoMapper.selectBrandName(brandID);
		}
		return name;
	}
}