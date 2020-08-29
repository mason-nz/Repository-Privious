package com.xyauto.domain;

import com.fasterxml.jackson.annotation.JsonProperty;

public class TestDemo {
	@JsonProperty("id")
	private String bid;
	private String name;
	private boolean isParent;
	private String pid;

	public String getId() {
		return bid;
	}

	public void setId(String id) {
		this.bid = id;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public boolean isParent() {
		return isParent;
	}

	public void setParent(boolean isParent) {
		this.isParent = isParent;
	}

	public String getpId() {
		return pid;
	}

	public void setPid(String pid) {
		this.pid = pid;
	}

	public TestDemo(String id, String name, boolean isParent, String pid) {
		super();
		this.bid = id;
		this.name = name;
		this.isParent = isParent;
		this.pid = pid;
	}

}
