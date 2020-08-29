package com.xyauto.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;

import com.xyauto.domain.UserDetailInfo;

@Mapper
public interface UserDetailInfoMapper {

	int insert(UserDetailInfo userDetailInfo);

	int deleteByPrimaryKey(Integer userID);

	int updateByPrimaryKey(UserDetailInfo userDetailInfo);

	UserDetailInfo selectByPrimaryKey(Integer userID);

	List<UserDetailInfo> selectAll();
}