package com.xyauto.test;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.springframework.test.context.junit4.SpringRunner;

import com.xyauto.util.Md5Utils;

@RunWith(SpringRunner.class)
public class MD5Test {

	@Before
	public void before() {
		System.out.println("测试开始");
	}

	@Test
	public void getMD5() {
		String pwd = Md5Utils.getMD5("123" + "29001" + "1234567890ABC", "UTF-8");
		System.out.println(pwd);
	}

	@After
	public void after() {
		System.out.println("测试结束");
	}
}
