import restaurantsApi from "~/api/restaurants/restaurantsApi";
import { GetInitialPropsBuilder } from "~/utils/GetInitialPropsBuilder";

import { Dashboard } from "~/views/Dashboard/Dashboard";

Dashboard.getInitialProps = new GetInitialPropsBuilder()
  .requireRole("RestaurantManager")
  .useState(async (state) => {
    const user = state.auth.user;

    const response = await restaurantsApi.getByManagerId(user.id);
    state.auth.restaurant = response.data;
  })
  .build();

export default Dashboard;
