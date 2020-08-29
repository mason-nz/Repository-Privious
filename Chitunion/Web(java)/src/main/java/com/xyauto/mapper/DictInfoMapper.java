package com.xyauto.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;

import com.xyauto.domain.DictInfo;

@Mapper
public interface DictInfoMapper {

	List<DictInfo> getDictInfoList();

}