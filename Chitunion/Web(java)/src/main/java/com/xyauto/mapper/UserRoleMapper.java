package com.xyauto.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Param;

import com.xyauto.domain.UserRole;

@Mapper
public interface UserRoleMapper {

	int insert(UserRole userRole);

	int deleteByPrimaryKey(@Param("userID") Integer userID, @Param("roleID") String roleID);

	int updateByPrimaryKey(UserRole userRole);

	UserRole selectByPrimaryKey(@Param("userID") Integer userID, @Param("roleID") String roleID);

	List<UserRole> selectAll();
}