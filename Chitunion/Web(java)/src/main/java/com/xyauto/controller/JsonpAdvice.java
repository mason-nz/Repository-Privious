package com.xyauto.controller;

import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.multipart.MultipartException;
import org.springframework.web.servlet.mvc.method.annotation.AbstractJsonpResponseBodyAdvice;

import com.xyauto.util.RestEntity;

import lombok.extern.slf4j.Slf4j;

@Slf4j
@ControllerAdvice
public class JsonpAdvice extends AbstractJsonpResponseBodyAdvice {
	public JsonpAdvice() {
		super("callback", "jsonp");
	}

	@ExceptionHandler(MultipartException.class)
	public @ResponseBody RestEntity<String> uploadExceptionHandler(Exception exception) {
		log.error(exception.getLocalizedMessage());
		return RestEntity.error("上传大小不能超过10M");
	}
}