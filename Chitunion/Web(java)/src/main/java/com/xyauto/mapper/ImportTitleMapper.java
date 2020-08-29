package com.xyauto.mapper;

import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Param;

import com.xyauto.domain.IpTitleInfo;
import com.xyauto.domain.TitleBasicInfo;

@Mapper
public interface ImportTitleMapper {
	void insertIntoBasicTitleDB(TitleBasicInfo tb);

	void insertIntoRelationDB(Integer pTitleId, Integer kidTitleId, Integer titleid);

	Integer selectTitleIdByName(@Param("titleName") String titleName);

	Integer selectRelationByPidAndKidAndbasicId(@Param("pTitleId") Integer pTitleId,
			@Param("kidTitleId") Integer kidTitleId, @Param("titleid") Integer titleid);

	void insertIntoRelationDB(IpTitleInfo ipTitleInfo);

	Integer selectRelationByKidAndbasicId(Integer kidTitleId, Integer titleid);
}
