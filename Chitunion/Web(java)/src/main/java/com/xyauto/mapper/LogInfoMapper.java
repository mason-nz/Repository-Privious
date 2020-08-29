package com.xyauto.mapper;

import org.apache.ibatis.annotations.Mapper;

import com.xyauto.domain.LogInfo;

@Mapper
public interface LogInfoMapper {

	void insertUserLog(LogInfo logInfo);

}