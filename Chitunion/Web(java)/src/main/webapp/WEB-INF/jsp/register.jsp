<%@ page language="java" contentType="text/html; charset=UTF-8"
	pageEncoding="UTF-8"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta charset="utf-8">
<meta name="description" content="描述文字，字数200内？">
<meta name="keywords" content="网站相关关键字，字数200内？">
<title>赤兔联盟平台</title>
<link rel="icon" href="${pageContext.request.contextPath }/images/favicon.ico" type="image/x-icon">
<link rel="stylesheet" type="text/css"
	href="${pageContext.request.contextPath }/css/reset.css" />
<link rel="stylesheet" type="text/css"
	href="${pageContext.request.contextPath }/css/layout.css" />
<script
	src="${pageContext.request.contextPath }/js/jquery.1.11.3.min.js"></script>
<script type="text/javascript"
	src="${pageContext.request.contextPath }/js/tab.js"></script>
	<script type="text/javascript" src="${pageContext.request.contextPath }/js/userInfoJS/register.js"></script>
</head>

<body>
	<!--顶部logo-->
	<div class="topBar">
	    <div class="topBox">
	        <a href="http://www.chitunion.com/index.html" class="topLogo"></a>
	        <div class="fl ml20 come">欢迎注册</div>
	        <div class="fr come">已有账号？ <a href="http://www.chitunion.com/" class="yellow">请登录</a></div>
	        <div class="clear"></div>
	    </div>
	</div>
	<!--中间内容-->
	<div class="register">
		<div class="tab2">
			<ul class="menu">
				<li class="active" onclick="addvice()">广告主</li>
				<li onclick="media()">媒体主</li>
			</ul>
			<div class="reg1">
				<div class="cont" style="margin: 20px 0px 20px 5px;">
					<ul>
						<li class="hra">手机号：</li>
						<li><input id="mobile29001" onblur="checkMobileReg(29001)" name="mobile" type="text" style="width:362px;"
							placeholder="请输入手机号" onchange="mobileChange(29001)" maxlength="11" onkeyup="value=value.replace(/[^\d]/g,'')" autocomplete="off"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="phoneMessage29001"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">验证码：</li>
						<li>
							<input id="validateCode29001" name="validateCode" onchange="mobileChange(29001)" onblur="checkImg(this.value,29001)" type="text" style="width: 125px;" placeholder="请输入验证码" autocomplete="off">
						</li>
						<li>
							<span>
								<img id="codeValidateImg29001" onclick="changeImage(29001)" />
							</span>
						</li>
						<li>
						 	<a onclick="changeImage(29001)" class="blue">换张图</a>
						</li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="message29001"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">短信验证码：</li>
						<li>
							<input id="mobileCode29001" onfocus="clearMesMessage(29001)" name="mobileCode" 
							onblur="checkPhone(29001)" type="text" style="width: 125px;"
							placeholder="请输入手机验证码" maxlength="6" autocomplete="off">
						</li>
						<li>
							<input id="getImgCode29001" type="button" class="obtain_no" 
							onclick="getMessageCode(this,29001)" value="获取验证码" />
						</li>
						<li>
							<input id="getImgMesssage29001" type="button"
							class="obtain" style="display: none;" />
						</li>
							<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="mesMessage29001"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">密码：</li>
						<li>
							<input id="pwd29001" name="pwd" onblur="checkPwd(29001)"
							type="password" style="width:362px;" placeholder="请输入密码（6-20位数字，字母）"
							autocomplete="off">
						</li>
							<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="pwdMessage29001"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">确认密码：</li>
						<li>
							<input id="pwdTwo29001" name="pwdTwo" type="password" onblur="checkPwdTwo(29001)" style="width:362px;"
							placeholder="请再次输入密码" autocomplete="off">
						</li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="pwdTwoMessage29001"></li>
						<div class="clear"></div>
					</ul>

					<ul style="margin-top:0px;">
						<li class="hra">&nbsp;</li>
						<li>
							<a class="button" id="tijiao29001"
							onclick="javascript:submitForm(29001);" href="javascript:void(0);" style="width: 178px">注册</a>
						</li>
						<div class="clear"></div>
					</ul>
					<ul class="mt35">
						<li class="hra">&nbsp;</li>
						<li><input name="" type="checkbox" value="" checked="checked" />
							我已阅读并同意 <a class="blue" href="javascript:void(0);" onclick="registerService()">服务条款</a></span></li>
						<div class="clear"></div>
					</ul>
				</div>
			</div>
			<div class="reg2">
				<div class="cont" style="margin: 20px 0px 20px 5px;">
					<ul>
						<li class="hra">手机号：</li>
						<li><input id="mobile29002" onchange="mobileChange(29002)" onblur="checkMobileReg(29002)"
							name="mobile" type="text" maxlength="11" onkeyup="value=value.replace(/[^\d]/g,'')" style="width:362px;"
							placeholder="请输入手机号" autocomplete="off"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="phoneMessage29002"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">验证码：</li>
						<li><input id="validateCode29002" name="validateCode" onchange="mobileChange(29002)"
							onblur="checkImg(this.value,29002)" type="text"
							style="width: 125px;" placeholder="请输入验证码" autocomplete="off"></li>
						<li><span><img id="codeValidateImg29002"
								onclick="changeImage(29002)" /></span></li>
						<li> 
							<a onclick="changeImage(29002)" class="blue">换张图</a></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="message29002"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">短信验证码：</li>
						<li>
							<input id="mobileCode29002" onfocus="clearMesMessage(29002)" name="mobileCode"
							onblur="checkPhone(29002)" type="text" style="width: 125px;"
							placeholder="请输入手机验证码" maxlength="6" autocomplete="off">
						</li>
						<li>
							<input id="getImgCode29002" type="button"  class="obtain_no"
							onclick="getMessageCode2(this,29002)" value="获取验证码" />
						</li>
						<li>
							<input id="getImgMesssage29002" type="button"
							class="obtain" style="display: none;" />
						</li>
					<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="mesMessage29002"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">密码：</li>
						<li><input id="pwd29002" name="pwd" onblur="checkPwd(29002)"
							type="password" style="width:362px;" placeholder="请输入密码（6-20位数字，字母）"
							autocomplete="off"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="pwdMessage29002"></li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">确认密码：</li>
						<li>
							<input id="pwdTwo29002" name="pwdTwo"
							onblur="checkPwdTwo(29002)" type="password" style="width:362px;"
							placeholder="请再次输入密码" autocomplete="off">
						</li>
						<div class="clear"></div>
					</ul>
					<ul>
						<li class="hra">&nbsp;</li>
						<li class="red" id="pwdTwoMessage29002"></li>
						<div class="clear"></div>
					</ul>

					<ul style="margin-top: 0px;">
						<li class="hra">&nbsp;</li>
						<li><a class="button" id="tijiao29002" href="javascript:void(0);"
							onclick="javascript:submitForm(29002);" style="width: 178px">注册</a></li>
						<div class="clear"></div>
					</ul>
					<ul class="mt35">
						 <li class="hra">&nbsp;</li>
						<li><input name="" type="checkbox" value="" checked="checked" />
							我已阅读并同意 <a class="blue" href="javascript:void(0);" onclick="registerService()">服务条款</a></span></li>
						<div class="clear"></div>
					</ul>
					<input type="hidden" id="roles">
					<div class="clear"></div>
				</div>
			</div>
		</div>
	</div>

	<div class="line2">&nbsp;</div>
	<!--底部-->
	<div id="footer2">
	    <div class="main">
<!-- 	        <p>IT系统中心 任何建议和意见，请进入 销售管理系统-反馈中心</p> -->
	        <p>@2016-2017 www.chitunion.com All Rights Reserved. 北京行圆汽车信息技术有限公司所有 京ICP备17011360号-3</p>
	    </div>
	</div>
	<div id="occlusion" style=" background-color:#eee"></div>
	<!--注册协议开始-->
	<div class="layer" id="layer" style="display: none;">
	    <div class="title">
	        <div class="fl">赤兔联盟用户服务协议</div>
	        <div class="clear"></div>
	    </div>
	    <div class="layer_con4">
	        <div class="scrollBar">
	            <p>尊敬的用户，欢迎注册成为赤兔联盟平台的用户。在注册前请仔细阅读如下服务条款：</p>
	            <h2>一、【声明与承诺】</h2>
	            <p>1.1赤兔联盟（以下简称“本平台”）运营商北京行圆汽车信息技术有限公司依据《赤兔联盟用户服务协议》（简称《协议》）的规定提供服务。用户必须完全同意本《协议》，才能够享受赤兔联盟提供的服务。用户在赤兔联盟注册成功即表示用户完全接受本《协议》的全部条款。注册用户前请务必认真阅读本《协议》，用户可以选择”接受“或”拒绝“本《协议》（未成年人接受本协议应取得监护人的认可）。用户只有在接受本《协议》的情况下，才有权使用赤兔联盟提供的相关服务。如您不同意本《协议》，则视为您放弃成为本平台上的用户，您将无法获得本平台提供的各项服务。如您接受《协议》，则视为本《协议》为全部接受。</p>
	            <p>1.2 本《协议》可由本网站随时更新，更新后的协议条款一旦公布即代替原来的协议条款，本平台不再另行通知，用户可在本平台自行查阅最新版协议条款。在本平台修改协议条款后，如果用户不接受修改后的条款，可立即停止使用本平台提供的服务，用户继续使用本平台提供的服务将被视为接受修改后的协议。</p>
	            <p>1.3  本《协议》适用于本平台提供的各种服务，但当您使用平台某一特定服务时，如该服务另有单独或特殊的《协议》或规则，您应遵守本《协议》及单独或特殊的《协议》或规则，如果单独或特殊的《协议》或规则与本《协议》发生冲突，以单独或特殊的《协议》或规则为准。</p>
	            <h2>二、【用户资格】</h2>
	            <p> 2.1、 只有符合下列条件之一的人员或实体才能申请成为本平台用户，可以使用本平台所提供的服务。</p>
	            <p> 	1).年满十八岁，并具有民事权利能力和民事行为能力的自然人；</p>
	            <p> 	2).未满十八岁，但监护人（包括但不仅限于父母）予以书面同意的自然人；</p>
	            <p> 2.2、根据中国法律或设立地法律、法规和/或规章成立并合法存在的公司、企事业单位、社团组织和其他组织。无民事行为能力人、限制民事行为能力人以及无经营或特定经营资格的组织不当注册为本平台用户或超过其民事权利或行为能力范围从事交易 的，其与本平台之间的协议自始无效，本平台一经发现，有权立即注销该用户，并追究其使用本平台"服务"的一切法律责任。</p>
	            <p>	2.3、用户必须提供真实姓名或公司名称，有明确的联系地址和联系电话。</p>
	            <h2>三、【用户权利与义务】</h2>
	            <p>	3.1、用户有权根据本《协议》及赤兔联盟发布的相关规则，利用赤兔联盟交易平台发布需求信息、查询媒介资源信息、投放广告、参加赤兔联盟的活动及有权享受赤兔联盟提供的其他的有关资讯及信息服务。</p>
	            <p>	3.2、用户有权根据需要更改登录密码和支付密码。用户应对以该用户名进行的所有活动和事件负全部责任。</p>
	            <p>	3.2、用户有权根据需要更改登录密码和支付密码。用户应对以该用户名进行的所有活动和事件负全部责任。</p>
	            <p>	3.2、用户有权根据需要更改登录密码和支付密码。用户应对以该用户名进行的所有活动和事件负全部责任。</p>
	            <p>	3.3、用户有义务确保向赤兔联盟提供的任何资料、注册信息真实准确，包括但不限于真实的公司名称、联系人姓名、身份证号、联系电话、地址、邮政编码等。保证赤兔联盟可以通过上述联系方式与用户进行联系。同时，用户也有义务在相关资料实际变更时及时更新有关注册资料。</p>
			    <p>	3.4、用户不得以任何形式擅自转让或授权他人（赤兔联盟工作人员除外）使用自己在赤兔联盟的用户帐号。
			    <p>	3.5、用户有义务确保在赤兔联盟交易平台下的订单信息的真实、准确，无误导性。
			    <p> 3.6、用户不得在赤兔联盟发布国家禁止信息、不得发布侵犯他人知识产权或其他合法权益的信息，也不得发布违背社会公共利益或公共道德的信息。
			    <p> 3.7、用户在赤兔联盟交易中应当遵守诚实信用原则，不得以不正当竞争方式扰乱网上交易秩序，不得从事与网上交易无关的不当行为，不得在交易平台上发布任何违法信息。
				<p> 3.8、用户承诺自己在使用赤兔联盟交易平台实施的所有行为遵守国家法律、法规和赤兔联盟的相关规定以及各种社会公共利益或公共道德。对于任何法律后果，用户将独立承担所有法律责任。
			   	<p> 3.9、用户在赤兔联盟平台交易过程中如与其他用户因交易产生纠纷，可以请求赤兔联盟从中予以协调。用户如发现其他用户有违法或违反本《协议》的行为，可以向赤兔联盟举报。如用户因网上交易与其他用户产生诉讼的，用户有权通过司法部门要求赤兔联盟提供相关资料。
			  	<p> 3.10、用户应承担因交易产生的相关费用，并依法纳税。
			  	<p> 3.11、未经赤兔联盟书面允许，用户不得将赤兔联盟的媒介信息以及在平台上所展示的任何信息以复制、修改、翻译等形式（包含但不限于以上形式）制作衍生作品、分发或公开展示。
			  	<p>  3.12、用户不得使用以下方式登录网站或破坏网站所提供的服务：
			      	<p>  1) 以机器人软件、蜘蛛软件、爬虫软件、刷屏软件或其它自动方式访问或登录赤兔联盟；
			     	<p>  2) 通过任何方式对赤兔联盟网站结构造成或可能造成不良影响的行为；
			     	<p>  3) 通过任何方式干扰或试图干扰赤兔联盟的交易行为；
			  	<p> 3.13、用户同意其提供给赤兔联盟的邮箱和手机接收来自赤兔联盟的信息，包括但不限于活动信息、交易信息、促销信息等。
			    <h2>四、【平台使用规范】</h2>
			    <p> 4.1、禁止发布、传送、传播、储存违反国家法律法规禁止的内容：   
			    <p> 4.2、违反宪法确定的基本原则的；   
			    <p> 4.3、危害国家安全，泄露国家秘密，颠覆国家政权，破坏国家统一的；  
			    <p> 4.4、损害国家荣誉和利益的；   
			    <p> 4.5、煽动民族仇恨、民族歧视，破坏民族团结的；   
			    <p> 4.6、破坏国家宗教政策，宣扬邪教和封建迷信的；   
			    <p> 4.7、散布谣言，扰乱社会秩序，破坏社会稳定的；   
			    <p> 4.8、散布淫秽、色情、赌博、暴力、恐怖或者教唆犯罪的；   
			    <p> 4.9、侮辱或者诽谤他人，侵害他人合法权益的；   
			    <p> 4.10、煽动非法集会、结社、游行、示威、聚众扰乱社会秩序；   
			 	<p> 4.11、以非法民间组织名义活动的；
			   	<p> 4.12、涉及近期敏感言论和敏感活动，抨击他人的产品或广告，进行负面推广；
			    <p> 4.13、含有法律、行政法规禁止的其他内容的。
			 	<p> 4.14、一旦本平台发现用户发布以上违法内容，本平台将不予审核通过，且有权时刻中止或删除活动，本平台有权利向公安、工商等相关政府机关举报，其后果由广告主自行承担。自用户发布违法内容起，视为用户违约，本平台相应的不再承担本《协议》下的义务。
			    <p> 4.15、若用户发布不实信息，由此带来不良后果，用户应承担相应责任，与本平台无关。 如用户违反国家法律法规、本《协议》或对本平台进行恶意攻击时，本平台将有权停止向用户提供服务而不需承担任何责任，如导致本平台遭受任何损害或遭受任何来自第三方的纠纷、诉讼、索赔要求,用户须向本平台赔偿相应的损失，如诉讼费、律师费、其他第三方费用、业务影响损失等费用，并且用户需对其违反《协议》所产生的一切后果负全部法律责任。
				<h2>五、【赤兔联盟的权利和义务】</h2>
				<p> 5.1、赤兔联盟仅为用户提供一个交易平台，用户必须保证所有信息真实有效合法。
			    <p> 5.2、赤兔联盟有义务在现有技术水平的基础上确保整个网上交易平台的正常运行，避免服务中断或将中断时间限制在最短时间内，保证用户网上交易活动的顺利进行。
			    <p> 5.3、赤兔联盟有义务对用户在注册使用赤兔联盟平台所遇到的问题及时作出解答。
			    <p> 5.4、赤兔联盟有权对用户的注册资料进行查阅，对存在任何问题或怀疑的注册资料，赤兔联盟有权发出通知询问用户并要求用户做出解释、改正，或不通知用户直接做出处罚、删除信息、删除账号等处理。
			    <p> 5.5、用户因在赤兔联盟上交易与其他用户产生质检纠纷的，用户同意赤兔联盟根据事前制定的系统质检流程对纠纷进行判断、裁决，在判断过程中，赤兔联盟有权通过电子邮件及电话联系向纠纷双方了解纠纷情况，并将根据了解的情况进行协调沟通，最终作出裁决。用户同意接受赤兔联盟裁决所导致的补款或扣款处理。
			    <p> 5.6、赤兔联盟没有义务对所有用户的注册资料、所有的交易行为以及与交易有关的其他事项进行事先审查，但如发生以下情形，赤兔联盟有权限制用户的活动、向用户核实有关资料、发出警告通知、暂时中止、无限期地中止及拒绝向该用户提供服务：
			        <p> 1) 用户违反本《协议》或因被提及而纳入本《协议》的文件；
			        <p> 2) 存在用户或其他第三方通知赤兔联盟，认为某个用户或具体交易事项存在违法或不当行为，并提供相关证据，而赤兔联盟无法联系到该用户核实该用户向赤兔联盟提供的任何资料；
			        <p> 3) 存在用户或其他第三方通知赤兔联盟，认为某个用户或具体交易事项存在违法或不当行为，并提供相关证据。赤兔联盟以普通非专业交易者的知识水平标准对相关内容进行判别，可以明显认为这些内容或行为可能对赤兔联盟用户或赤兔联盟造成财务损失或法律责任。
			    <p> 5.7、根据国家法律法规，本《协议》的内容和赤兔联盟所掌握的事实依据，可以认定用户存在违法或违反本《协议》行为以及在赤兔联盟平台上的其他不当行为，赤兔联盟有权在赤兔联盟平台以网络发布形式公布用户的违法行为，并有权随时作出删除相关信息、终止服务提供等处理，而无须征得用户的同意。
			    <p> 5.8、赤兔联盟有权在不通知用户的前提下删除或采取其他限制性措施处理下列信息：包括但不限于以规避费用为目的；以炒作信用为目的；存在欺诈等恶意或虚假内容；与网上交易无关或不是以交易为目的；存在恶意下单或其他试图扰乱正常交易秩序因素；该信息违反公共利益或可能严重损害赤兔联盟和其他用户合法利益的。
				<h2>六、【服务的中断和终止 】</h2>
			    <p> 6.1、在本平台未向用户收取相关服务费用的情况下，本平台可自行全权决定以任何理由 (包括但不限于本平台认为用户已违反本《协议》的字面意义和精神等) 终止对用户的服务，并不再保存用户在本平台的全部资料（包括但不限于用户信息、资源信息、交易信息等）。同时本平台可自行全权决定，在发出通知或不发出通知的情况下，随时停止提供全部或部分服务。服务终止后，本平台没有义务为用户保留原用户资料或与之相关的任何信息，或转发任何未曾阅读或发送的信息给用户或第三方。 
			    <p> 6.2、如用户向本平台提出注销本平台注册用户身份，需经本平台审核同意，由本平台注销该注册用户，用户即解除与本平台的协议关系，但本平台仍保留下列权利：   
			     <p>    1)、用户注销后，本平台有权保留该用户的资料,包括但不限于以前的用户资料、交易记录等。  
			     <p>    2)、用户注销后，如用户在注销前在本平台交易平台上存在违法行为或违反本《协议》的行为，本平台仍可行使本《协议》所规定的权利。 
			    <p> 6.3、如存在下列情况，本平台可以通过注销用户的方式终止服务：     
			      <p> 	1)、在用户违反本《协议》相关规定时，本平台有权终止向该用户提供服务。本平台将在中断服务时通知用户。但如该用户在被本平台终止提供服务后，再一次直接或间接或以他人名义注册为本平台用户的，本平台有权再次单方面终止为该用户提供服务；     
			      <p> 	2)、一旦本平台发现用户注册资料中主要内容是虚假的，本平台有权随时终止为该用户提供服务；      
			  	  <p> 	3)、用户出现作弊行为，网站可根据情况作出处理，甚至注销用户。
			    <p> 6.4、其它本平台认为需终止服务的情况。
				<h2>七、【知识产权】</h2>
			    <p> 7.1、赤兔联盟及赤兔联盟所使用的任何相关软件、程序、内容，包括但不限于作品、图片、档案、资料、网站构架、网站版面的安排、网页设计、经由赤兔联盟或媒体主向用户呈现的广告或资讯，均由赤兔联盟或其它权利人依法享有相应的知识产权，包括但不限于著作权、商标权、专利权或其它专属权利等，受到相关法律的保护。未经赤兔联盟或权利人明示授权，用户保证不修改、出租、出借、出售、散布赤兔联盟及赤兔联盟所使用的上述任何资料和资源，或根据上述资料和资源制作成任何种类物品。
			    <p> 7.2、赤兔联盟授予用户不可转移及非专属的使用权，使用户可以通过计算机使用赤兔联盟的目标代码（以下简称"软件"），但用户不得且不得允许任何第三方，复制、修改、创作衍生作品、进行还原工程、反向组译，或以其它方式破译或试图破译源代码，或出售、转让"软件"或对"软件 "进行再授权，或以其它方式移转"软件"之任何权利。用户同意不以任何方式修改"软件"，或使用修改后的"软件"。
			    <p> 7.3、用户不得经由非赤兔联盟所提供的界面使用赤兔联盟。
				<h2> 八、【不可抗力】</h2>
			    <p> 8.1、因不可抗力或者其他意外事件，使得本《协议》的履行不可能、不必要或者无意义的，双方均不承担责任。本《协议》所称之不可抗力亦指不能预见、不能避免并不能克服的客观情况，包括但不限于战争、台风、水灾、火灾、雷击或地震、罢工、暴动、法定疾病、黑客攻击、网络病毒、电信部门技术管制、政府行为或任何其它自然或人为造成的灾难等客观情况。
				<h2>九、【争议解决方式】</h2>
			    <p> 9.1、本《协议》及其修订本的有效性、履行和与本《协议》及其修订本效力有关的所有事宜，将受中华人民共和国法律管辖，任何争议仅适用中华人民共和国法律。
			    <p> 9.2、因使用本平台服务所引起与本平台的任何争议，均应提交本公司所在地有管辖权的法院管辖。
	        </div>
	        <div class="keep" style="text-align: center">
	            <span><a onclick="offLayer()" href="javascript:void(0);" class="button" style="width:100px">确定</a></span>
	        </div>
	    </div>
	</div>
	<!--注册协议结束-->
	<div style="height:50px"></div>
</body>
</html>
