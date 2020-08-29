package com.xyauto.mapper;

import java.util.List;
import java.util.Map;

import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Param;

import com.xyauto.domain.CarTitleListQuery;
import com.xyauto.domain.IpTitleInfo;
import com.xyauto.domain.MediaInfoQuery;
import com.xyauto.domain.TitleBasicInfo;

@Mapper
public interface TitleBasicInfoMapper {
	int deleteByPrimaryKey(Integer titleid);

	int insert(TitleBasicInfo record);

	TitleBasicInfo selectByPrimaryKey(Integer titleid);

	List<TitleBasicInfo> selectAll();

	int updateByPrimaryKey(TitleBasicInfo record);

	List<TitleBasicInfo> selectAllTitle();

	List<TitleBasicInfo> test(Map<String, Integer> map);

	List<IpTitleInfo> getRelationByTitleID(Integer titleId);

	MediaInfoQuery selectWxInfoById(Integer mediaId);

	MediaInfoQuery selectWbInfoById(Integer mediaId);

	MediaInfoQuery selectAppInfoById(Integer mediaId);

	MediaInfoQuery selectVideoInfoById(Integer mediaId);

	MediaInfoQuery selectBroadCastInfoById(Integer mediaId);

	String selectBrandName(Integer brandID);

	CarTitleListQuery selectSerialName(@Param("brandID") Integer brandID, @Param("serialID") Integer serialID);
}