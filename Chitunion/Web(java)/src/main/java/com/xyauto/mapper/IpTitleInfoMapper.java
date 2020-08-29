package com.xyauto.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;

import com.xyauto.domain.IpTitleInfo;

@Mapper
public interface IpTitleInfoMapper {
	int deleteByPrimaryKey(Integer recid);

	int insert(IpTitleInfo record);

	IpTitleInfo selectByPrimaryKey(Integer recid);

	List<IpTitleInfo> selectAll();

	int updateByPrimaryKey(IpTitleInfo record);
}