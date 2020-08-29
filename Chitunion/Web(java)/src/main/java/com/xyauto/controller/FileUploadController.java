package com.xyauto.controller;

import java.io.IOException;
import java.io.InputStream;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Controller;
import org.springframework.util.MimeTypeUtils;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.multipart.MultipartFile;

import com.xyauto.util.RestEntity;
import com.xyauto.util.SysConstants;

@Controller
public class FileUploadController {

	@Value("${com.xyauto.controller.localPath}")
	private String localPath;
	@Value("${com.xyauto.controller.uploadImgPath}")
	private String uploadImgPath;
	@Value("${com.xyauto.controller.domainName}")
	private String domainName;
	@Value("${com.xyauto.controller.downloadImgPath}")
	private String downloadImgPath;

	@PostMapping("uploadImg")
	public @ResponseBody RestEntity<Map<String, Object>> handleFileUpload(@RequestParam("pic") MultipartFile file)
			throws IOException {
		if (file.isEmpty()) {
			return new RestEntity<Map<String, Object>>(99, "上传不能为空");
		}
		if (file.getSize() > SysConstants.MAX_UPLOAD_IMG) {
			return new RestEntity<Map<String, Object>>(99, "上传大小不能超过2M");
		}
		int index = file.getOriginalFilename().lastIndexOf(".");
		if (index == -1) {
			return new RestEntity<Map<String, Object>>(99, "文件格式不正确");
		}
		// 验证文件扩展名
		String fileContentType = file.getContentType();
		if (!("image\\/x-png".equals(fileContentType) || // 兼容IE8一下类型
				MimeTypeUtils.IMAGE_GIF_VALUE.equals(fileContentType)
				|| MimeTypeUtils.IMAGE_PNG_VALUE.equals(fileContentType)
				|| MimeTypeUtils.IMAGE_JPEG_VALUE.equals(fileContentType))) {
			return new RestEntity<Map<String, Object>>(99, "不是标准图片格式");
		}
		// GIF PNG 4位
		int codeLength = 4;
		// 如果是JPG 3位
		if (MimeTypeUtils.IMAGE_JPEG_VALUE.equals(fileContentType)) {
			codeLength = 3;
		}
		// 验证文件头
		InputStream is = file.getInputStream();
		byte[] b = new byte[10];
		is.read(b, 0, b.length);
		StringBuilder stringBuilder = new StringBuilder();
		if (null == b || b.length <= 0) {
			return new RestEntity<Map<String, Object>>(99, "不是标准图片格式");
		}
		for (int i = 0; i < codeLength; i++) {
			int v = b[i] & 0xFF;
			String hv = Integer.toHexString(v);
			if (hv.length() < 2) {
				stringBuilder.append(0);
			}
			stringBuilder.append(hv);
		}
		// 比较文件头
		String fileCode = stringBuilder.toString();
		if (!(SysConstants.JPG.equals(fileCode) || SysConstants.PNG.equals(fileCode)
				|| SysConstants.GIF.equals(fileCode))) {
			return new RestEntity<Map<String, Object>>(99, "不是标准图片格式");
		}
		// 获取扩展名
		String extName = file.getOriginalFilename().substring(index).toLowerCase();
		// 生成图片名称
		String fileName = UUID.randomUUID().toString();
		// 生成当前日期路径
		String datePath = new SimpleDateFormat("yyyy-M-d-H").format(new Date()).replaceAll("-", "\\\\");
		Path path = Paths.get(localPath + uploadImgPath + "\\" + datePath);
		// 判断目录是否存在
		if (Files.notExists(path)) {
			Files.createDirectories(path);
		}
		Files.copy(file.getInputStream(), path.resolve(fileName + extName));
		String tempPath = downloadImgPath + "/" + datePath + "/" + fileName + extName;
		Map<String, Object> result = new HashMap<>();
		result.put("fileSize", file.getSize());
		result.put("domainName", domainName);
		result.put("finaPath", tempPath.replaceAll("\\\\", "/"));
		return RestEntity.data(result);
	}
}
