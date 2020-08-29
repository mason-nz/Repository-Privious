package com.xyauto.mapper;

import java.util.List;
import java.util.Map;

import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Param;

import com.xyauto.domain.CarTitleListQuery;
import com.xyauto.domain.MediaTitleListQuery;
import com.xyauto.domain.TitleBasicInfo;
import com.xyauto.domain.TitleCarMapping;
import com.xyauto.domain.TitleMediaMapping;

@Mapper
public interface TitleMediaMappingMapper {
	int deleteByPrimaryKey(Integer recid);

	int insert(TitleMediaMapping record);

	TitleMediaMapping selectByPrimaryKey(Integer recid);

	List<TitleMediaMapping> selectAll();

	int updateByPrimaryKey(TitleMediaMapping record);

	void insertSelective(TitleMediaMapping record);

	List<Integer> getTitleIdByMediaIdAndMediaType(@Param("mediaType") Integer mediaType,
			@Param("mediaId") Integer mediaId);

	List<TitleBasicInfo> getTitlesByMediaIdAndMediaType(@Param("mediaType") Integer mediaType,
			@Param("mediaId") Integer mediaId);

	void delTitleOfMedia(@Param("mediaType") Integer mediaType, @Param("mediaId") Integer mediaId);

	// 根据不同类型的媒体id查询该媒体对应的标签名集合
	List<String> getTitleNamesByWeiXinMediaId(@Param("mediaId") Integer mediaId);

	List<String> getTitleNamesByPCAPPMediaId(@Param("mediaId") Integer mediaId);

	List<String> getTitleNamesByWeiboMediaId(@Param("mediaId") Integer mediaId);

	List<String> getTitleNamesByVideoMediaId(@Param("mediaId") Integer mediaId);

	List<String> getTitleNamesByBroadcastMediaId(@Param("mediaId") Integer mediaId);

	// 根据媒体id查询与它账号一致的媒体id，平台媒体不存在这个问题
	List<Integer> getOtherMediaIdSameWithWeiXinNumberByMediaId(@Param("mediaId") Integer mediaId);

	List<Integer> getOtherMediaIdSameWithWeiboNumberByMediaId(@Param("mediaId") Integer mediaId);

	List<Integer> getOtherMediaIdSameWithVideoNumberByMediaId(@Param("mediaId") Integer mediaId);

	List<Integer> getOtherMediaIdSameWithBroadcastNumberByMediaId(@Param("mediaId") Integer mediaId);

	// 下面方法，获取五种未标签的媒体
	List<MediaTitleListQuery> getWeiXinWithOutTitle(Map<String, Integer> map);

	List<MediaTitleListQuery> getAppWithOutTitle(Map<String, Integer> map);

	List<MediaTitleListQuery> getWeiBoWithOutTitle(Map<String, Integer> map);

	List<MediaTitleListQuery> getVideoWithOutTitle(Map<String, Integer> map);

	List<MediaTitleListQuery> getBroadCastWithOutTitle(Map<String, Integer> map);

	List<MediaTitleListQuery> getAllMediaWithOutTitle(Map<String, Integer> map);

	// 以下方法，获取各种类型未打标签的媒体

	List<MediaTitleListQuery> getAllMediaWithTitleList(Map<String, Object> map);

	List<CarTitleListQuery> getAllCarBrandListWithTitle(Map<String, Object> map);

	// 查询所有打了标签的品牌列表
	List<CarTitleListQuery> getBrandsWithTitle();

	// 查询未打标签的品牌列表
	List<CarTitleListQuery> getAllBrandsWithOutTitle(Map<String, Object> map);

	// 获取没有标签的品牌（仅品牌）
	List<CarTitleListQuery> getJustBrandWithOutTitle();

	List<CarTitleListQuery> queryCarBrandByTitleName(@Param("titleName") String titleName);

	List<CarTitleListQuery> queryCarBrandByTitleName(Map<String, Object> map);

	// List<TitleMediaMapping> queryMediaByTitleName(@Param("titleName") String
	// titleName);

	List<TitleMediaMapping> queryMediaByTitleName(Map<String, Object> map);

	MediaTitleListQuery getWeiXinMediaNameByMediaId(@Param("mediaId") Integer mediaId);

	MediaTitleListQuery getPCAPPMediaNameByMediaId(@Param("mediaId") Integer mediaId);

	MediaTitleListQuery getWeiBoMediaNameByMediaId(@Param("mediaId") Integer mediaId);

	MediaTitleListQuery getVideoMediaNameByMediaId(@Param("mediaId") Integer mediaId);

	MediaTitleListQuery getBroadCastMediaNameByMediaId(@Param("mediaId") Integer mediaId);

	// 根据品牌和车系id获取标签集合
	List<TitleBasicInfo> getTitlesByBrandIdAndSerialId(@Param("brandID") Integer brandID,
			@Param("serialID") Integer serialID);
	// 根据品牌id和车系id插入标签

	void deleteAllTitleAboutCarBrand(@Param("brandID") Integer brandID, @Param("serialID") Integer serialID);

	void addTitleToCarBrandByBrandIdAndSerialId(TitleCarMapping record);

}