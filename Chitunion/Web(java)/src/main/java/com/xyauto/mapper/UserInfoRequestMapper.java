package com.xyauto.mapper;

import java.util.List;

import org.apache.ibatis.annotations.Mapper;

import com.xyauto.domain.AEAutocompleter;
import com.xyauto.domain.UserInfoOrder;
import com.xyauto.domain.UserInfoRequest;

@Mapper
public interface UserInfoRequestMapper {
	List<UserInfoRequest> getUserInfoReq(UserInfoRequest userInfoRequest);

	int upUserInfoByRquest(List<UserInfoRequest> userInfoRequest);

	List<UserInfoRequest> checkMobileRegister(UserInfoRequest userInfoRequest);

	void registerUserInfo(UserInfoRequest userInfoRequest);

	int registerUserRole(UserInfoRequest userInfoRequest);

	void updateUserInfo(UserInfoRequest userInfoRequest);

	List<UserInfoRequest> getUserInfoList(UserInfoRequest userInfoRequest);

	List<UserInfoRequest> getAERole(UserInfoRequest userInfoRequest);

	void updateUserAuthAE(List<UserInfoRequest> userInfoRequest);

	List<UserInfoRequest> getUserOperationList(UserInfoRequest userInfoRequest);

	List<UserInfoRequest> selectAddUserList(UserInfoRequest userInfoRequest);

	List<AEAutocompleter> getAEList(String aeRole);

	List<UserInfoOrder> getUserInfoOrder(String strWhere);

	int updateUserPWD(UserInfoRequest userInfo);
}