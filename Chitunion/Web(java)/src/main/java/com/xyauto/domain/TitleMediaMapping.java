package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class TitleMediaMapping {
	private Integer recId;
	private Integer titleId;
	private Integer mediaType;
	private Integer mediaId;
	private Integer status;
	private Date createTime;
	private Integer createUserId;
}
