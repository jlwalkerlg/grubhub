import { RegisterRestaurant } from "~/views/RegisterRestaurant/RegisterRestaurant";
import { GetInitialPropsBuilder } from "~/lib/GetInitialPropsBuilder";

RegisterRestaurant.getInitialProps = new GetInitialPropsBuilder()
  .guestOnly()
  .build();

export default RegisterRestaurant;
