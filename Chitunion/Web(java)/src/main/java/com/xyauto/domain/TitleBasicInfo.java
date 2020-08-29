package com.xyauto.domain;

import java.util.Date;

import com.fasterxml.jackson.annotation.JsonProperty;

public class TitleBasicInfo {
	@JsonProperty("id")
	private Integer titleid;

	private String name;

	private Integer type; // 0,1,2 母，子，普通

	private Integer status;

	private Date createtime;

	private Integer createuserid;

	private Integer pipId;

	private Integer rp; // rp是一组母子关系（包括母ip，子ip 和 子ip，普通标签）中绝对的母ip

	private String nodeName = "";

	public String getNodeName() {
		return nodeName;
	}

	public void setPipId(Integer pipId) {
		this.pipId = pipId;
	}

	public Integer getpId() {
		if (null != this.pipId) {
			return this.pipId;
		}
		return 0;
	}

	public Integer getTitleid() {
		return titleid;
	}

	public void setTitleid(Integer titleid) {
		this.titleid = titleid;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public Integer getType() {
		return type;
	}

	public void setType(Integer type) {
		this.type = type;
	}

	public Integer getRp() {
		return rp;
	}

	public void setRp(Integer rp) {
		this.rp = rp;
	}

	public Integer getStatus() {
		return status;
	}

	public void setStatus(Integer status) {
		this.status = status;
	}

	public Date getCreatetime() {
		return createtime;
	}

	public void setCreatetime(Date createtime) {
		this.createtime = createtime;
	}

	public Integer getCreateuserid() {
		return createuserid;
	}

	public void setCreateuserid(Integer createuserid) {
		this.createuserid = createuserid;
	}
}