<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagLayout.ascx.cs" Inherits="BitAuto.ISDC.CC2012.Web.TemplateManagement.TagLayout" %>
<style type="text/css">
    .pointer
    {
    }
</style>
<script type="text/javascript">
    function setTab(obj, cursel, n) {
        var this$ = $(obj);
        this$.addClass('hover').siblings().removeClass('hover');
        this$.closest('.hw_tab').find('div[did="con_two_' + cursel + '"]').siblings().hide().end().show();        
    }

    function checkLeft(obj) {
        var this$ = $(obj);
        var ulTagC$ = this$.siblings('ul');
        if (ulTagC$.children('li').length <= 11) {
            return false;
        };

        var rr$ = ulTagC$.children('li[rr=1]'); // $('#ulTagC >li[rr=1]');
        var ll$ = ulTagC$.children('li[ll=1]'); //$('#ulTagC >li[ll=1]');
        var rn$ = ulTagC$.next();
        if (ll$.prev().length > 0) {
            rr$.removeAttr("rr").hide(200).prev().attr("rr", 1).show(500);
            ll$.removeAttr("ll").prev().attr("ll", 1).show(500);

            //后端加粗
            if (rn$.hasClass('r_arrow_g')) {
                rn$.removeClass('r_arrow_g');
            }

            if (ll$.prev().prev().length > 0) {
                if (this$.hasClass('l_arrow_g')) {
                    this$.removeClass('l_arrow_g');
                }
            } else {
                this$.addClass('l_arrow_g');
            }
        } else {
            this$.addClass('l_arrow_g');
        }
        return false;
    }
    function checkRight(obj) {
        var this$ = $(obj);

        var ulTagC$ = this$.siblings('ul');
        if (ulTagC$.children('li').length <= 11) {
            return false;
        };
     
        var rr$ = ulTagC$.children('li[rr=1]'); // $('#ulTagC >li[rr=1]');
        var ll$ = ulTagC$.children('li[ll=1]'); //$('#ulTagC >li[ll=1]');
        var ln$ = ulTagC$.prev();
        if (rr$.next().length > 0) {

            //左边界去掉加灰
            if (ln$.hasClass('l_arrow_g')) {
                ln$.removeClass('l_arrow_g');
            }

            ll$.removeAttr("ll").hide(500).next().attr("ll", 1).show();
            rr$.removeAttr("rr").next().attr("rr", 1).show(500);

            if (rr$.next().next().length > 0) {
                if (this$.hasClass('r_arrow_g')) {
                    this$.removeClass('r_arrow_g');
                }
            } else {
                this$.addClass('r_arrow_g');
            }
        } else {
            this$.addClass('r_arrow_g');
        }
        return false;
    }

    function toggleUl(obj) {
        $(obj).toggleClass("toggle").closest('.bq1').next().find('.Tab2').toggle();
        if ($(obj).hasClass('hide')) {
            $(obj).addClass('toggle').removeClass('hide');
        } else {
            $(obj).addClass('hide').removeClass('toggle');
        }
    }

    $(function () {
        //$('a[name="linkUporDown"]:eq(0)').trigger("click");
        $('a.aEdit:eq(0)').trigger("click");
        
        if ($('.r_arrow').length > 0) {
            $('.r_arrow').each(function (i, dom) {
                var this$ = $(dom);
                if (this$.prev().children().length > 11) {
                    this$.removeClass("r_arrow_g");
                } else {
                    this$.addClass("r_arrow_g");
                }
            });
        }
    });
</script>
<label id="lbT" runat="server">
</label>

