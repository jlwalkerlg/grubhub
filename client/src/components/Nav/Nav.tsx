import Link from "next/link";
import { useRouter } from "next/router";
import React, { useState } from "react";
import { useQueryClient } from "react-query";
import useAuth from "~/api/users/useAuth";
import useLogout from "~/api/users/useLogout";
import CloseIcon from "~/components/Icons/CloseIcon";
import LogoutIcon from "~/components/Icons/LogoutIcon";
import MenuIcon from "~/components/Icons/MenuIcon";
import RegisterIcon from "~/components/Icons/RegisterIcon";
import RestaurantMenuIcon from "~/components/Icons/RestaurantMenuIcon";
import { usePreventBodyScroll } from "~/services/usePreventBodyScroll";
import DashboardIcon from "../Icons/DashboardIcon";
import OrdersIcon from "../Icons/OrdersIcon";
import UserCircleIcon from "../Icons/UserCircleIcon";
import { useToasts } from "../Toaster/Toaster";
import styles from "./Nav.module.css";

const Nav: React.FC = () => {
  const { addToast } = useToasts();

  const [isOpen, setIsOpen] = React.useState(false);
  const toggleNav = () => {
    setIsOpen(!isOpen);
  };

  usePreventBodyScroll(isOpen);

  const { user, isLoggedIn, isLoading } = useAuth();
  const { mutate: logout } = useLogout();

  const router = useRouter();
  const queryClient = useQueryClient();

  const onLogout = async () => {
    logout(null, {
      onSuccess: async () => {
        await router.push("/");
        queryClient.clear();
      },

      onError: (error) => {
        addToast(error.detail);
      },
    });
  };

  const [isDropdownOpen, setIsDropdownOpen] = useState(false);

  const openDropdown = () => setIsDropdownOpen(true);
  const closeDropdown = () => setIsDropdownOpen(false);

  return (
    <nav className="fixed w-full z-30 top-0 bg-white" id="nav">
      <div className="shadow-md relative z-10">
        <div className="container flex items-center h-16 py-3">
          <Link href="/">
            <a className="font-semibold">
              <RestaurantMenuIcon className="w-6 h-6 inline" />
              <span className="align-middle ml-1">FOOD</span>
              <span className="text-primary align-middle ml-1">SNAP</span>
            </a>
          </Link>

          <button
            className="md:hidden ml-auto rounded-sm w-8 h-8 bg-primary p-1 text-white"
            onClick={toggleNav}
          >
            {isOpen ? (
              <CloseIcon className="w-6 h-6" />
            ) : (
              <MenuIcon className="w-6 h-6" />
            )}
          </button>

          {!isLoading && isLoggedIn && (
            <div className="hidden md:block ml-auto">
              <div
                className="relative cursor-pointer"
                onMouseEnter={openDropdown}
                onMouseLeave={closeDropdown}
              >
                <div className="flex items-center uppercase font-medium px-2 py-2">
                  <UserCircleIcon className="h-5" />
                  <span className="ml-2">{user.firstName}</span>
                </div>

                <div
                  className={`absolute top-100 pt-3 right-0 ${
                    isDropdownOpen ? "" : "hidden"
                  }`}
                >
                  <div
                    className={`relative bg-white text-gray-700 px-2 rounded shadow whitespace-nowrap ${styles["dropdown"]}`}
                  >
                    <Link href="/account">
                      <a className="block px-4 py-3 hover:text-primary border-b border-gray-200">
                        Your Account
                      </a>
                    </Link>
                    <Link href="/order-history">
                      <a className="block px-4 py-3 hover:text-primary border-b border-gray-200">
                        Your Orders
                      </a>
                    </Link>
                    {user.role === "RestaurantManager" && (
                      <Link href="/dashboard">
                        <a className="block px-4 py-3 hover:text-primary border-b border-gray-200">
                          Restaurant Dashboard
                        </a>
                      </Link>
                    )}
                    <button
                      type="button"
                      onClick={onLogout}
                      className="block px-4 py-3 hover:text-primary w-full text-left"
                    >
                      Logout
                    </button>
                  </div>
                </div>
              </div>
            </div>
          )}

          {!isLoading && !isLoggedIn && (
            <>
              <Link href="/login">
                <a className="hidden md:block ml-auto px-2 py-2 uppercase font-medium hover:text-primary">
                  Login
                </a>
              </Link>
              <Link href="/register">
                <a className="hidden md:block px-2 py-2 uppercase font-medium hover:text-primary">
                  Register
                </a>
              </Link>
              <Link href="/restaurants/register">
                <a className="btn btn-primary hidden md:block ml-4">
                  Register Restaurant
                </a>
              </Link>
            </>
          )}
        </div>
      </div>

      <div
        className={`md:hidden ${
          isOpen ? "" : "hidden"
        } fixed bottom-0 w-full h-full top-0 mt-16 left-0 bg-gray-100`}
      >
        <ul className="container mt-5">
          {!isLoading && isLoggedIn && (
            <li>
              <Link href="/account">
                <a className="block py-2 uppercase font-medium hover:text-primary">
                  <OrdersIcon className="w-6 h-6 inline" />
                  <span className="ml-2 align-middle">Your Account</span>
                </a>
              </Link>
              <Link href="/order-history">
                <a className="block py-2 uppercase font-medium hover:text-primary">
                  <OrdersIcon className="w-6 h-6 inline" />
                  <span className="ml-2 align-middle">Your Orders</span>
                </a>
              </Link>
              {user.role === "RestaurantManager" && (
                <Link href="/dashboard">
                  <a className="block py-2 uppercase font-medium hover:text-primary">
                    <DashboardIcon className="w-6 h-6 inline" />
                    <span className="ml-2 align-middle">
                      Restaurant Dashboard
                    </span>
                  </a>
                </Link>
              )}
              <button
                type="button"
                onClick={onLogout}
                className="block py-2 uppercase font-medium hover:text-primary w-full text-left"
              >
                <LogoutIcon className="w-6 h-6 inline" />
                <span className="ml-2 align-middle">Logout</span>
              </button>
            </li>
          )}

          {!isLoading && !isLoggedIn && (
            <>
              <li>
                <Link href="/login">
                  <a className="block py-2 uppercase font-medium hover:text-primary">
                    <LogoutIcon className="w-6 h-6 inline" />
                    <span className="ml-2 align-middle">Login</span>
                  </a>
                </Link>
              </li>
              <li>
                <Link href="/register">
                  <a className="block py-2 uppercase font-medium hover:text-primary">
                    <RegisterIcon className="w-6 h-6 inline" />
                    <span className="ml-2 align-middle">Register</span>
                  </a>
                </Link>
              </li>
              <li>
                <div className="mt-5 pt-1 border-solid border-gray-400 border-t-2">
                  <Link href="/restaurants/register">
                    <a className="btn btn-primary mt-6">Register Restaurant</a>
                  </Link>
                </div>
              </li>
            </>
          )}
        </ul>
      </div>
    </nav>
  );
};

export default Nav;
