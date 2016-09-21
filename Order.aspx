<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Order.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Order Maker
    </h2>
    <h5>
        Use this upload option to Send customer data to Environmental Versatile Intuitive
        Logistics Inc (EVIL inc).
    </h5>
    <br />
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:Timer ID="Timer1" runat="server" Interval="500" OnTick="Timer1_Tick" Enabled="False">
    </asp:Timer>
    <div id="divStatus" runat="server" style="display: ">
        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" />
                <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
            <ContentTemplate>
                <table>
                    <td>
                        Data File
                    </td>
                    <td>
                        <asp:FileUpload ID="fleCustomerData" CssClass="FileUpload" runat="server" />
                    </td>
                    <td>
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"
                            CssClass="submitButton" />
                    </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblStatus" runat="server" Text="........"></asp:Label>
                        </td>
                    </tr>
                </table>
                <fieldset>
                    <legend>Order Processing Status</legend>
                    <div style="width: 100%; height: 250px; overflow-y: scroll;">
                        <asp:Label ID="lblLog" runat="server"></asp:Label>
                    </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
