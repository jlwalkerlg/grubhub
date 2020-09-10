import { GetInitialPropsBuilder } from "~/lib/GetInitialPropsBuilder";
import { Login } from "~/views/Login/Login";

Login.getInitialProps = new GetInitialPropsBuilder().guestOnly().build();

export default Login;
