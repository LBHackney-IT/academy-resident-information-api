CREATE SCHEMA IF NOT EXISTS dbo;

CREATE TABLE dbo.hbhousehold (
	claim_id int NULL,
	house_id smallint NULL,
	last_upd int NULL,
	from_date timestamp NULL,
	to_date varchar(37) NULL,
	inc_supp_ind smallint NULL,
	claim_type_ind smallint NULL,
	addr1 varchar(35) NULL,
	addr2 varchar(35) NULL,
	addr3 varchar(32) NULL,
	addr4 varchar(32) NULL,
	post_code varchar(10) NULL,
	find_addr1 varchar(16) NULL,
	find_addr2 varchar(16) NULL,
	reassess_ind smallint NULL,
	srr_exempt smallint NULL,
	lp_protect_ind smallint NULL,
	cbl_protect_ind smallint NULL,
	bereavement_override smallint NULL,
	is_claim_date timestamp NULL,
	is_award_date timestamp NULL,
	is_reject_date timestamp NULL,
	uprn varchar(12) NULL,
	away_address_ind smallint NULL,
	shared_accom_ind smallint NULL,
	is_mid_week_split smallint NULL,
	locality_code varchar(6) NULL,
	districtmove_id int NULL,
	dmp_ind smallint NULL,
	group_id int NULL,
	ben_cap_level_ind smallint NULL,
	uc_dhp_ind smallint NULL,
	category_ind int NULL,
	fa_ind int NULL,
	CONSTRAINT PK_hbhousehold PRIMARY KEY (claim_id, house_id)
);

CREATE TABLE dbo.hbmember (
	claim_id int NULL,
	house_id smallint NULL,
	member_id smallint NULL,
	person_ref int NULL,
	name varchar(32) NULL,
	surname varchar(32) NULL,
	title varchar(4) NULL,
	forename varchar(32) NULL,
	find_name varchar(32) NULL,
	type smallint NULL,
	birth_date varchar(37) NULL,
	gender smallint NULL,
	carer_ind smallint NULL,
	disable_ind smallint NULL,
	hospital_ind smallint NULL,
	meals_ind smallint NULL,
	shared_percent real NULL,
	student_ind smallint NULL,
	hours_worked varchar(6) NULL,
	cared_for_ind smallint NULL,
	school_ind smallint NULL,
	nino varchar(10) NULL,
	age_ind smallint NULL,
	self_empl_ind smallint NULL,
	incapable_of_work smallint NULL,
	weeks_28 smallint NULL,
	weeks_52 smallint NULL,
	hold_risk_sub_grp_cas timestamp NULL,
	residential_acc smallint NULL,
	bereaved_date timestamp NULL,
	bea_end_date timestamp NULL,
	fepow_disregard numeric(14, 2) NULL,
	mat_leave_ccp_ind smallint NULL,
	holocaust_disregard numeric(14, 2) NULL,
	srplumpsum_disregard numeric(14, 2) NULL,
	chb_end_date timestamp NULL,
	deceased_date timestamp NULL,
	eight_week_ind smallint NULL,
	num_child_under_1 smallint NULL,
	num_child_under_16 smallint NULL,
	num_youth_16_18 smallint NULL,
	num_dis_child_prem smallint NULL,
	num_enhan_dis_child_prem smallint NULL,
	ccp_prison_hosp_ind smallint NULL,
	ccp_rem_work_ind smallint NULL,
	advantageous_change_ind smallint NULL,
	not_payable_ind smallint NULL,
	non_dep_gc_sc_ind smallint NULL,
	non_dep_easement_ind smallint NULL,
	sanction_rate numeric(14, 2) NULL,
	easement_date timestamp NULL,
	disregard_50 smallint NULL,
	non_dep_grp smallint NULL,
	party_ref int NULL,
	spd_ignore_date timestamp NULL,
	unique_pupil_num varchar(15) NULL,
	severely_mentally_impaired_ind smallint NULL,
	period_sanction int NULL,
	sanction_type int NULL,
	sanction_rate_type int NULL,
	CONSTRAINT PK_hbmember PRIMARY KEY (claim_id, member_id, house_id),
	CONSTRAINT UC_hbmember UNIQUE (claim_id, house_id)
);

CREATE TABLE dbo.hbclaim(
    division_id                 smallint,
    claim_id                    int,
    last_upd                    int,
    corresp_ind                 smallint,
    work_area                   varchar(4),
    name                        varchar(72),
    application_date            timestamp,
    freq_units                  smallint,
    freq_period                 smallint,
    review_date                 timestamp,
    ext_review_date             timestamp,
    cancel_date                 timestamp,
    commence_ind                smallint,
    cancel_ind                  smallint,
    deter_days_pt               smallint,
    deter_days_ct               smallint,
    deter_days_cb               smallint,
    diary_date                  timestamp,
    disc_ctax_award             numeric(14, 2),
    disc_ctax_cease_date        timestamp,
    disc_ctax_comm_date         timestamp,
    disc_rent_award             numeric(14, 2),
    disc_rent_cease_date        timestamp,
    disc_rent_comm_date         timestamp,
    forward_surname             varchar(32),
    forward_forename            varchar(32),
    forward_title               varchar(4),
    forward_addr1               varchar(35),
    forward_addr2               varchar(35),
    forward_addr3               varchar(32),
    forward_addr4               varchar(32),
    forward_post_code           varchar(10),
    ignore_subsidy_ind          smallint,
    debtor_1off_amt             numeric(14, 2),
    debtor_period_amt           numeric(14, 2),
    debtor_total_amt            numeric(14, 2),
    is_award_date               timestamp,
    is_reject_date              timestamp,
    nino                        varchar(10),
    hb93_ref                    varchar(14),
    pay_freq_ind                smallint,
    giro_code                   varchar(3),
    pt_days_1st_pay             smallint,
    after_14_days_ind           smallint,
    ro_form_ind                 smallint,
    ro_form_sent_date           timestamp,
    ro_refer_date               timestamp,
    pt_paid_to_date             timestamp,
    ct_paid_to_date             timestamp,
    ctax_paid_to_date           timestamp,
    sprs_ctax_notif_ind         smallint,
    sprs_rent_notif_ind         smallint,
    sprs_llord_notif_ind        smallint,
    suspend_pay_ind             smallint,
    unsafe_addr_ind             smallint,
    urgent_pay_ind              smallint,
    user_code1                  varchar(2),
    user_code2                  varchar(1),
    ctax_year_posted            smallint,
    rent_year_posted            smallint,
    check_digit                 varchar(1),
    change_lock                 int,
    diary_code                  varchar(2),
    notes_db_handle             varchar(76),
    second_lang_ind             smallint,
    ctax_trans_id               smallint,
    rent_trans_id               smallint,
    pending_id                  smallint,
    poss_is_claim_ind           smallint,
    is_cancel_date              timestamp,
    ep_applied_date             timestamp,
    last_completed_date         timestamp,
    last_applied_date           timestamp,
    arrears_ind                 smallint,
    prev_post_amt               numeric(14, 2),
    pot_violent                 smallint,
    fsm_ind                     smallint,
    fraud_28_days_ind           int,
    cl_cancel_ind               smallint,
    cl_creditor_id              varchar(16),
    cl_award_paid               numeric(14, 2),
    clothing_award_tot          numeric(14, 2),
    fplp_warning_ind            smallint,
    withhold_pay_ind            smallint,
    withdrawn_ind               smallint,
    cbl_warning_ind             smallint,
    ba_visit_date               timestamp,
    la_visit_date               timestamp,
    risk_sub_group              varchar(3),
    old_risk_sub_group          varchar(3),
    last_iv_commence_date       timestamp,
    last_iv_completed_date      timestamp,
    in_yr_chk_ind               smallint,
    contact_tel_no              varchar(18),
    contact_tel_type            smallint,
    contact_time                varchar(40),
    fraud_proved_ind            smallint,
    risk_warn_ind               smallint,
    determine_risk_ind          smallint,
    ethnic_id                   varchar(4),
    paid_on_acc_ind             smallint,
    applic_recvd_date           timestamp,
    is_claim_date               timestamp,
    multiple_occ_ind            smallint,
    cct_file_status             smallint,
    client_status               smallint,
    poa_decision_date           timestamp,
    payment_stats_ind           smallint,
    last_review_date            timestamp,
    tenant_recovery             smallint,
    manual_risk_group           varchar(1),
    no_auto_inyrchk             smallint,
    employ_start_date           timestamp,
    max_weekly_recovery_amt     numeric(14, 2),
    max_recovery_calc_ind       smallint,
    std_recovery_calc_ind       smallint,
    hbr_visit_date              timestamp,
    sig_birth_ind               smallint,
    cl_marked_for_ass           smallint,
    ro_deter_req                smallint,
    fraud_proved_review_date    timestamp,
    multiple_occ_review_date    timestamp,
    susp_reason_code            varchar(3),
    ha_ass_id                   int,
    thbs_comm_ind               smallint,
    thbs_canc_ind               smallint,
    received_by                 varchar(3),
    warn_code                   varchar(3),
    warn_user_id                varchar(8),
    forward_uprn                varchar(12),
    away_address_ind            smallint,
    equal_access_code           varchar(3),
    summary_scan_ind            smallint,
    temp_lha_ind                smallint,
    lha_review_ind              smallint,
    edbn_ind                    smallint,
    last_edbn_id                int,
    status_ind                  int,
    delay_suspend_date          timestamp,
    hb_suspend_date             timestamp,
    ctb_suspend_ind             int,
    ctb_suspend_date            timestamp,
    ctb_suspend_reason          varchar(3),
    ll_suspend_reason           varchar(3),
    hb_suspend_iv_id            int,
    ctb_suspend_iv_id           int,
    last_iv_contact_date        timestamp,
    old_addr1                   varchar(35),
    old_addr2                   varchar(35),
    old_addr3                   varchar(32),
    old_addr4                   varchar(32),
    old_post_code               varchar(10),
    archived_ind                smallint,
    fp_to_ll_ind                smallint,
    fp_ll_name                  varchar(32),
    fp_addr1                    varchar(35),
    fp_addr2                    varchar(35),
    fp_addr3                    varchar(32),
    fp_addr4                    varchar(32),
    fp_post_code                varchar(10),
    suppress_payment_qa         int,
    qa_process                  int,
    last_qaass_id               int,
    new_liability_date          timestamp,
    new_liability_tog           smallint,
    first_contact_tog           smallint,
    coa_ind                     smallint,
    first_contact_date          timestamp,
    curr_language               smallint,
    rent_liability_chg_ind      int,
    last_assess_id              int,
    hb_claim_source             varchar(2),
    ctb_claim_source            varchar(2),
    hb_end_reason               varchar(3),
    ctb_end_reason              varchar(3),
    lsvt_date                   timestamp,
    dhp_debtor_1off_amt         numeric(14, 2),
    archive_flag                smallint,
    non_archive_reason          int,
    set_date                    timestamp,
    lha_excess_payee            int,
    lha_excess_rent_arrears_amt numeric(14, 2),
    lha_excess_payee_ref        varchar(16),
    lha_excess_reassess_ind     int,
    old_calc_amt                numeric(14, 2),
    protected_excess_amt        numeric(14, 2),
    lha_review_timestamp        int,
    sprs_tp_notif_ind           smallint,
    rent_change_liability_date  timestamp,
    risk_score                  int,
    vf_risk_date                timestamp,
    campaign_code               int,
    hb_vf_find_error_ind        int,
    ctb_vf_find_error_ind       int,
    is_conv_case                smallint,
    conv_ta_amt                 numeric(14, 2),
    conv_ta_override_ind        smallint,
    conv_ta_override_rsn_ind    smallint,
    ib_to_esa_conv_date         timestamp,
    tp_ind                      smallint,
    tp_end_date                 timestamp,
    previous_lha_amount         numeric(14, 2),
    rent_dhp_to_date            timestamp,
    ctax_dhp_to_date            timestamp,
    spl_instr_db_handle         varchar(76),
    cts_conv_ta_amt             numeric(14, 2),
    atlas_notified_date         timestamp,
    dhp_pt_paid_to_date         timestamp,
    dhp_ct_paid_to_date         timestamp,
    uc_dhp_start_date           timestamp
);

CREATE TABLE dbo.ctaccount (
	division_id smallint NULL,
	account_ref int NULL,
	account_cd varchar(1) NULL,
	lead_liab_name varchar(32) NULL,
	lead_liab_pos smallint NULL,
	for_addr1 varchar(32) NULL,
	for_addr2 varchar(32) NULL,
	for_addr3 varchar(32) NULL,
	for_addr4 varchar(32) NULL,
	for_postcode varchar(8) NULL,
	for_district varchar(17) NULL,
	paymeth_code varchar(5) NULL,
	paymeth_type smallint NULL,
	prev_paymeth varchar(5) NULL,
	prev_paymeth_type smallint NULL,
	employee_ind smallint NULL,
	employee_no varchar(12) NULL,
	swipe_issue_date timestamp NULL,
	swipe_card_request smallint NULL,
	hb_sar smallint NULL,
	fast_track smallint NULL,
	equal_access_code varchar(3) NULL,
	sched_code varchar(6) NULL,
	person_status varchar(1) NULL,
	transmission_date timestamp NULL,
	lead_liab_title varchar(8) NULL,
	lead_liab_forename varchar(32) NULL,
	lead_liab_middlename varchar(32) NULL,
	lead_liab_surname varchar(32) NULL,
	lead_consent_ind smallint NULL,
	for_addr_last_updated int NULL,
	hb_archive_ind smallint NULL,
	party_ref int NULL,
	for_addr_verified smallint NULL,
	last_updated_int int NULL,
	prohibit_code varchar(2) NULL,
	prohibit_end timestamp NULL,
	prohibit_set timestamp NULL,
	prohibit_user varchar(8) NULL,
	prohibit_online_spd smallint NULL,
	for_addr_abroad smallint NULL,
	documerge_excl_ind smallint NULL
);

CREATE TABLE dbo.ctproperty (
	division_id smallint NULL,
	property_ref varchar(18) NULL,
	fund_no smallint NULL,
	parish_code varchar(8) NULL,
	street_code varchar(8) NULL,
	house_id varchar(8) NULL,
	addr1 varchar(35) NULL,
	addr2 varchar(35) NULL,
	addr3 varchar(32) NULL,
	addr4 varchar(32) NULL,
	postcode varchar(8) NULL,
	grid_ref varchar(20) NULL,
	old_property_ref varchar(20) NULL,
	property_type varchar(40) NULL,
	owner_name varchar(32) NULL,
	own_addr1 varchar(32) NULL,
	own_addr2 varchar(32) NULL,
	own_addr3 varchar(32) NULL,
	own_addr4 varchar(32) NULL,
	own_postcode varchar(8) NULL,
	owner_away_ind smallint NULL,
	owner_liable smallint NULL,
	crown_ind smallint NULL,
	council_ha_ind varchar(1) NULL,
	institution smallint NULL,
	planning_ind smallint NULL,
	planning_desc varchar(30) NULL,
	planning_granted timestamp NULL,
	survey_code varchar(6) NULL,
	workarea varchar(6) NULL,
	walking_order varchar(6) NULL,
	deleted_ind smallint NULL,
	deleted_eff timestamp NULL,
	band_010493 varchar(1) NULL,
	cc_payable numeric(14, 2) NULL,
	transit_93 numeric(14, 2) NULL,
	transit_94 numeric(14, 2) NULL,
	transit_95 numeric(14, 2) NULL,
	transit_96 numeric(14, 2) NULL,
	transit_97 numeric(14, 2) NULL,
	inhibit_archive smallint NULL,
	owner_code varchar(4) NULL,
	uprn varchar(12) NULL,
	wardcode varchar(6) NULL,
	owner_title varchar(8) NULL,
	owner_forename varchar(32) NULL,
	owner_middlename varchar(32) NULL,
	owner_surname varchar(32) NULL,
	inhibit_llpg smallint NULL,
	llpg_xref_key varchar(14) NULL,
	owner_addr_verified smallint NULL,
	last_updated_int int NULL,
	rented_prop smallint NULL,
	own_telno varchar(20) NULL,
	own_email_addr varchar(128) NULL,
	no_beds smallint NULL,
	annexe_ind smallint NULL,
	main_prop_ref varchar(18) NULL,
	own_is_charity smallint NULL,
	own_addr_abroad smallint NULL
);

CREATE TABLE dbo.syemail (
  applic_id varchar(2),
  division_id smallint,
  reference_id int,
  person_type smallint,
  person_type_seq_no smallint,
  email_addr varchar(128),
  email_doc_ind smallint,
  email_fail_cnt smallint,
  email_start_date timestamp,
  email_end_date timestamp,
  last_updated_int int,
  creditor_id varchar(16),
  ben_ctax_ref int,
  earrg_start_date timestamp,
  earrg_end_date timestamp,
  earrg smallint
);

CREATE TABLE dbo.syphone (
  applic_id varchar(2),
  ref_type smallint,
  ref varchar(20),
  phonetype1 smallint,
  phonenum1 varchar(20),
  findphone1 varchar(20),
  phonetype2 smallint,
  phonenum2 varchar(20),
  findphone2 varchar(20),
  phonetype3 smallint,
  phonenum3 varchar(20),
  findphone3 varchar(20),
  phonetype4 smallint,
  phonenum4 varchar(20),
  findphone4 varchar(20),
  email varchar(64),
  unsol_tc smallint,
  notes varchar(300)
);

CREATE TABLE dbo.ctoccupation (
	account_ref int,
	property_ref varchar(18),
	occupation_date timestamp,
	vacation_date timestamp,
	transit_ind smallint,
	live_ind smallint,
	last_updated_int int
);

create index ctoccupation_account_ref
	on dbo.ctoccupation (account_ref);

create index ctoccupation_vacation_date
	on dbo.ctoccupation (vacation_date);
