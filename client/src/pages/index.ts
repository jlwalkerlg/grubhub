import { GetInitialPropsBuilder } from "~/utils/GetInitialPropsBuilder";
import { Home } from "~/views/Home/Home";

Home.getInitialProps = new GetInitialPropsBuilder().build();

export default Home;
