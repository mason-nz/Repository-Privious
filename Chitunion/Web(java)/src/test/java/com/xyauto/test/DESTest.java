package com.xyauto.test;

import java.net.URLEncoder;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.test.context.junit4.SpringRunner;

import com.xyauto.util.DesUtil;
import com.xyauto.util.Md5Utils;

@RunWith(SpringRunner.class)
public class DESTest {

	@Before
	public void before() {
		System.out.println("测试开始");
	}

	@Test
	public void getDES()  {
//            try {
//				String test = "13";  
//				DesUtil des = new DesUtil();//自定义密钥  
//				System.out.println("加密前的字符："+test);  
//				System.out.println("加密后的字符："+des.encode(test)); 
//				System.out.println("解密后的字符："+des.decode(des.encode(test)));
//				System.out.println("URLEncoder:"+URLEncoder.encode(des.encode(test), "UTF-8"));
//			} catch (Exception e) {
//				e.printStackTrace();
//			}  
		Long min = null;
		if (min >= 3) {// 验证码过期
			System.out.println("pp");
		} 
	}

	@After
	public void after() {
		System.out.println("测试结束");
	}
}
