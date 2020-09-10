import { GetInitialPropsBuilder } from "~/lib/GetInitialPropsBuilder";
import { Home } from "~/views/Home/Home";

Home.getInitialProps = new GetInitialPropsBuilder().build();

export default Home;
