package com.xyauto.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;

import com.xyauto.domain.AreaInfo;

@Mapper
public interface AreaInfoMapper {
	List<AreaInfo> getAreaInfoList();
}