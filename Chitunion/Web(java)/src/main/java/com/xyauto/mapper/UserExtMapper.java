package com.xyauto.mapper;

import org.apache.ibatis.annotations.Mapper;
import org.apache.ibatis.annotations.Param;

import com.xyauto.domain.UploadFileInfo;
import com.xyauto.domain.UserDetailInfo;
import com.xyauto.domain.UserExt;

@Mapper
public interface UserExtMapper {

	UserExt infoAndDetail(Integer id);

	UserExt infoPassAndAEName(Integer id);

	Integer countEmployeeNumber(String string);

	Integer countTrueName(String string);

	Integer countMobile(String string);

	Integer countAEUserID(Integer userID);

	Integer countMobileAndCategory(@Param("mobile") String mobile, @Param("category") String category);

	UploadFileInfo selectUserUpload(@Param("type") Integer type, @Param("tableName") String tableName,
			@Param("userID") Integer userID);

	Integer userDetailInfoUpdateOrInsert(UserDetailInfo userDetailInfo);
}