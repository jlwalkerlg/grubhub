import { GetInitialPropsBuilder } from "~/utils/GetInitialPropsBuilder";

import { Dashboard } from "~/views/Dashboard/Dashboard";

Dashboard.getInitialProps = new GetInitialPropsBuilder().requireAuth().build();

export default Dashboard;
