package com.xyauto.domain;

import java.util.Date;

import lombok.Data;

@Data
public class IpTitleInfo {
	private Integer recId;
	private Integer pIp;
	private Integer subIp;
	private Integer relation;
	private Integer titleId;
	private Integer status;
	private Date createTime;
	private Integer createUserId;
	private String thisTitleName;
	private String pTitleName;
	private String subTitleName;
	private String relationName;
	private String showStr;

	public String getShowStr() {
		StringBuilder sb = new StringBuilder();
		if (!"".equals(this.pTitleName) && null != this.pTitleName) {
			sb.append(this.pTitleName + "-");
		}
		if (!"".equals(this.relationName) && null != this.relationName) {
			sb.append(this.relationName + "-");
		}
		if (!"".equals(this.subTitleName) && null != this.subTitleName) {
			sb.append(this.subTitleName + "-");
		}
		if (!"".equals(this.thisTitleName) && null != this.thisTitleName) {
			sb.append(this.thisTitleName);
		}
		return sb.toString();
	}
}
