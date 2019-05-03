
public string GetFBCSQLString(long rrcno)
        {
            return "select a.ReferralSubmittedDate As 'Date', b.referraltypedisplay AS 'Value', 'Referral' AS 'Type' from dbo.ServiceFBCReferrals as a, dbo.ServiceFBCReferralTypeLookUp as b, dbo.ServiceFBCIntake as c where a.referraltypeid = b.id and c.id = a.ServiceFBCIntakeID and c.rrcNo = " + rrcno.ToString() + " union SELECT AppointmentDate,AppointmentStatus, 'Appointment' FROM dbo.ServiceFBCAppointments as a, dbo.ServiceFBCReferrals as b, dbo.ServiceFBCIntake as c WHERE a.ServiceFBCReferralsID = b.id and c.id = b.ServiceFBCIntakeID and c.rrcNo = " + rrcno.ToString() + " union SELECT IntakeDate, 'Intake', 'Intake' FROM dbo.ServiceFBCIntake WHERE RRCNo = " + rrcno.ToString() + " and (CreatedDate >= DATEADD(dd, -1, GETDATE()) or ModifiedDate >= DATEADD(dd, -1, GETDATE()))";
        }

        public string GetInHouseSQLString(long rrcno)
        {
            return "select a.ServiceDate AS 'Date', b.Serviceneedsdisplay AS 'Value' from dbo.ServiceInHouseService as a, dbo.InHouseServiceLookup as b where a.inhouseserviceid = b.id and a.rrcNo =  " + rrcno.ToString() + " and a.ServiceDate >= DATEADD(dd, -1, GETDATE())";
        }

        public string GetMMUSQLString(long rrcno)
        {
            return "select a.ServiceDate AS 'Date', b.serviceproviderdisplay AS 'Value' from dbo.ServiceMMUService as a, dbo.ServiceMMUServiceProviderLookUp as b where a.ServiceProviderID = b.id and a.rrcNo =  " + rrcno.ToString() + " and a.ServiceDate >= DATEADD(dd, -1, GETDATE())";
        }

        public string GetBHSQLString(long rrcno)
        {
            return "select a.EnrollmentDate AS 'Date', b.ReferredDisplay AS 'Value', 'Referral' AS 'Type' from dbo.ServiceBHSUTSReferrals as a, dbo.ServiceBHSUTSReferralLookUp as b, dbo.ServiceBHIntake as c where a.BHSUTSReferralID = b.id and c.id = a.ServiceBHIntakeID and c.rrcNo =  " + rrcno.ToString() + " Union select a.EnrollmentDate, b.ReferredDisplay, 'Referral' from dbo.ServiceBHMHReferrals as a, dbo.ServiceBHMHReferralLookUp as b, dbo.ServiceBHIntake as c where a.BHMHReferralID = b.id and c.id = a.ServiceBHIntakeID and c.rrcNo =  " + rrcno.ToString() + " union SELECT IntakeDate  , 'Intake', 'Intake' FROM dbo.ServiceBHIntake WHERE RRCNo = " + rrcno.ToString() + " and(CreatedDate >= DATEADD(dd, -1, GETDATE()) or ModifiedDate >= DATEADD(dd, -1, GETDATE()))";
        }
        public string GetORSSQLString(long rrcno)
        {
            return "select a.ReferralSubmittedDate AS 'Date', b.referraltypedisplay AS 'Value', 'Referral'AS 'Type' from dbo.ServiceORSReferrals as a, dbo.ServiceORSReferralTypeLookUp as b, dbo.ServiceORSIntake as c where a.referraltypeid = b.id and c.id = a.ServiceORSIntakeID and c.rrcNo =  " + rrcno.ToString() + " union SELECT AppointmentDate   ,AppointmentStatus, 'Appointment' FROM dbo.ServiceORSAppointments as a, dbo.ServiceORSReferrals as b, dbo.ServiceORSIntake as c WHERE a.ServiceORSReferralsID = b.id and c.id = b.ServiceORSIntakeID and c.rrcNo =  " + rrcno.ToString() + " union SELECT IntakeDate , 'Intake', 'Intake' FROM dbo.ServiceORSIntake WHERE RRCNo =  " + rrcno.ToString() + " and(CreatedDate >= DATEADD(dd, -1, GETDATE()) or ModifiedDate >= DATEADD(dd, -1, GETDATE()))";
        }
        public string GetSSASQLString(long rrcno)
        {
            return "select c.AppointmentDate AS 'Date', b.BenefitDisplay AS 'Value', 'Benefit'AS 'Type' from dbo.ServiceSSABenefits as a, dbo.ServiceSSABenefitLookUp as b, dbo.ServiceSSAIntake as c where a.ServiceSSAIntakeID = c.id and a.BenefitID = b.id and c.rrcNo =  " + rrcno.ToString() + " union select a.ReferralRequestedDate, b.ServiceDisplay, 'Referral' from dbo.ServiceSSAReferrals as a, dbo.ServiceSSAServiceProviderLookUp as b, dbo.ServiceSSAIntake as c where a.ServiceProviderID = b.id and c.id = a.ServiceSSAIntakeID and c.rrcNo =  " + rrcno.ToString() + " and ReferralRequestedDate >= DATEADD(dd, -1, GETDATE()) union SELECT ApplicationDate , 'Intake', 'Intake' FROM dbo.ServiceSSAIntake WHERE RRCNo =  " + rrcno.ToString() + " and(CreatedDate >= DATEADD(dd, -1, GETDATE()) or ModifiedDate >= DATEADD(dd, -1, GETDATE()))";
        }


        public Dictionary<string, List<SummaryResults>> GetClientSummary2(long RRCNo)
        {
            Dictionary<string, List<SummaryResults>> everything = new Dictionary<string, List<SummaryResults>>();
            List<SummaryResults> BH = new List<SummaryResults>();
            List<SummaryResults> FBC = new List<SummaryResults>();
            List<SummaryResults> ORS = new List<SummaryResults>();
            List<SummaryResults> SSA = new List<SummaryResults>();
            List<SummaryResults> InHouse = new List<SummaryResults>();
            List<SummaryResults> MMU = new List<SummaryResults>();

            using (AppsRTSEntities dbContext = new AppsRTSEntities())
            {
                ORS = dbContext.Database.SqlQuery<SummaryResults>(GetORSSQLString(RRCNo)).ToList<SummaryResults>();

                FBC = dbContext.Database.SqlQuery<SummaryResults>(GetFBCSQLString(RRCNo)).ToList<SummaryResults>();

                SSA = dbContext.Database.SqlQuery<SummaryResults>(GetSSASQLString(RRCNo)).ToList<SummaryResults>();

                BH = dbContext.Database.SqlQuery<SummaryResults>(GetBHSQLString(RRCNo)).ToList<SummaryResults>();

                InHouse = dbContext.Database.SqlQuery<SummaryResults>(GetInHouseSQLString(RRCNo)).ToList<SummaryResults>();

                MMU = dbContext.Database.SqlQuery<SummaryResults>(GetMMUSQLString(RRCNo)).ToList<SummaryResults>();
            }
            everything.Add("BH", BH);
            everything.Add("FBC", FBC);
            everything.Add("ORS", ORS);
            everything.Add("SSA", SSA);
            everything.Add("InHouse", InHouse);
            everything.Add("MMU", MMU);
            return everything;
        }

        public class SummaryResults
        {
            public Nullable<DateTime> Date { get; set; }
            public string Value { get; set; }
            public string Type { get; set; }
        }
