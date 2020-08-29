package com.xyauto.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;

import com.xyauto.domain.UserInfo;

@Mapper
public interface UserInfoMapper {

	int insert(UserInfo userInfo);

	int deleteByPrimaryKey(Integer id);

	int updateByPrimaryKey(UserInfo userInfo);

	UserInfo selectByPrimaryKey(Integer id);

	List<UserInfo> selectAll();

}