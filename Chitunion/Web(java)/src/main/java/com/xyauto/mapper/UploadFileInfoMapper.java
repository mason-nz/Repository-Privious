package com.xyauto.mapper;

import com.xyauto.domain.UploadFileInfo;
import java.util.List;

import org.apache.ibatis.annotations.Mapper;

@Mapper
public interface UploadFileInfoMapper {

	int deleteByPrimaryKey(Integer recID);

	int insert(UploadFileInfo record);

	UploadFileInfo selectByPrimaryKey(Integer recID);

	List<UploadFileInfo> selectAll();

	int updateByPrimaryKey(UploadFileInfo record);
}