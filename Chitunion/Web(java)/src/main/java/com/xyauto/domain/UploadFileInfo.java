package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class UploadFileInfo {

	private Integer recID;
	private Integer type;
	private String relationTableName;
	private Integer relationID;
	private String filePah;
	private String fileName;
	private String extendName;
	private Integer fileSize;
	private Date createTime;
	private Integer creaetUserID;
}