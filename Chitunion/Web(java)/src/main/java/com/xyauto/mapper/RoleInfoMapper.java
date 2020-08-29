package com.xyauto.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;

import com.xyauto.domain.RoleInfo;

@Mapper
public interface RoleInfoMapper {

	int insert(RoleInfo roleInfo);

	int deleteByPrimaryKey(String roleID);

	int updateByPrimaryKey(RoleInfo roleInfo);

	RoleInfo selectByPrimaryKey(String roleID);

	List<RoleInfo> selectAll();
}