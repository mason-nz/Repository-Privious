package com.xyauto.util;

import java.io.Serializable;

import lombok.Data;

@Data
public class RestEntity<T> implements Serializable {

	private static final long serialVersionUID = -2905194169090130143L;

	public static final int OK = 1; // 操作成功
	public static final int ERROR = 2; // 接口异常
	private int status; // 异常编码
	private String message; // 消息
	private T result; // 传输数据

	public RestEntity() {
		this.status = OK;
		this.message = "操作成功";
	}

	public RestEntity(int status, String message) {
		this.status = status;
		this.message = message;
	}

	public static RestEntity<String> success(String successInfo) {
		return new RestEntity<String>(OK, successInfo);
	}

	public static RestEntity<String> error(String errorInfo) {
		return new RestEntity<String>(ERROR, errorInfo);
	}

	public static <T> RestEntity<T> data(T t) {
		RestEntity<T> rd = new RestEntity<>();
		rd.setResult(t);
		return rd;
	}
}
