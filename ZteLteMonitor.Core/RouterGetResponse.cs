namespace ZteLteMonitor.Core
{

    public class RouterPostResponse
    {
        public string? result { get; set; }
    }

    public class RouterGetResponse
    {
        public string? modem_main_state { get; set; }
        public string? ppp_status { get; set; }

        //public string? modem_main_state { get; set; }
        public string? pin_status { get; set; }
        public string? opms_wan_mode { get; set; }
        public string? loginfo { get; set; }
        public string? new_version_state { get; set; }
        public string? current_upgrade_state { get; set; }
        public string? is_mandatory { get; set; }
        public string? wifi_dfs_status { get; set; }        
        public string? signalbar { get; set; }
        public string? network_type { get; set; }
        public string? network_provider { get; set; }
        //public string? ppp_status { get; set; }
        public string? ipsec_status { get; set; }
        public string? EX_SSID1 { get; set; }
        public string? sta_ip_status { get; set; }
        public string? EX_wifi_profile { get; set; }
        public string? m_ssid_enable { get; set; }
        public string? RadioOff { get; set; }
        public string? SSID1 { get; set; }
        public string? wifi_onoff_state { get; set; }
        public string? wifi_chip1_ssid1_ssid { get; set; }
        public string? wifi_chip2_ssid1_ssid { get; set; }
        public string? wifi_chip1_ssid1_access_sta_num { get; set; }
        public string? wifi_chip2_ssid1_access_sta_num { get; set; }
        public string? simcard_roam { get; set; }
        public string? lan_ipaddr { get; set; }
        public string? wifi_access_sta_num { get; set; }
        public string? station_mac { get; set; }
        public string? battery_charging { get; set; }
        public string? battery_vol_percent { get; set; }
        public string? battery_value { get; set; }
        public string? battery_pers { get; set; }
        public string? spn_name_data { get; set; }
        public string? spn_b1_flag { get; set; }
        public string? spn_b2_flag { get; set; }
        public string? realtime_tx_bytes { get; set; }
        public string? realtime_rx_bytes { get; set; }
        public string? realtime_time { get; set; }
        public string? realtime_tx_thrpt { get; set; }
        public string? realtime_rx_thrpt { get; set; }
        public string? monthly_rx_bytes { get; set; }
        public string? monthly_tx_bytes { get; set; }
        public string? monthly_time { get; set; }
        public string? date_month { get; set; }
        public string? data_volume_limit_switch { get; set; }
        public string? data_volume_limit_size { get; set; }
        public string? data_volume_alert_percent { get; set; }
        public string? data_volume_limit_unit { get; set; }
        public string? roam_setting_option { get; set; }
        public string? upg_roam_switch { get; set; }
        public string? ssid { get; set; }
        public string? wifi_enable { get; set; }
        public string? wifi_5g_enable { get; set; }
        public string? check_web_conflict { get; set; }
        public string? dial_mode { get; set; }
        public string? wifi_onoff_func_control { get; set; }
        public string? ppp_dial_conn_fail_counter { get; set; }
        public string? wan_connect_status { get; set; }
        public string? l2tp_status { get; set; }
        public string? radius_status { get; set; }
        public string? factory_mode { get; set; }
        public string? wan_lte_ca { get; set; }
        public string? rivacy_read_flag { get; set; }
        public string? pppoe_status { get; set; }
        public string? dhcp_wan_status { get; set; }
        public string? static_wan_status { get; set; }
        public string? sms_received_flag { get; set; }
        public string? sts_received_flag { get; set; }
        public string? sms_unread_num { get; set; }


        public string? wan_ipaddr { get; set; } // requires cookies
    }
}
