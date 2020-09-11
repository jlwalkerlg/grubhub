import { RegisterRestaurant } from "~/views/RegisterRestaurant/RegisterRestaurant";
import { GetInitialPropsBuilder } from "~/utils/GetInitialPropsBuilder";

RegisterRestaurant.getInitialProps = new GetInitialPropsBuilder()
  .guestOnly()
  .build();

export default RegisterRestaurant;
