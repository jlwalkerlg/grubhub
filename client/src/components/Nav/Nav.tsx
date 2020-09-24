import React from "react";
import Link from "next/link";

import RestaurantMenuIcon from "~/components/Icons/RestaurantMenuIcon";
import LogoutIcon from "~/components/Icons/LogoutIcon";
import RegisterIcon from "~/components/Icons/RegisterIcon";
import CloseIcon from "~/components/Icons/CloseIcon";
import MenuIcon from "~/components/Icons/MenuIcon";
import useAuth from "~/store/auth/useAuth";
import { useRouter } from "next/router";
import DashboardIcon from "../Icons/DashboardIcon";

const Nav: React.FC = () => {
  const router = useRouter();
  const { isLoggedIn, user, logout } = useAuth();

  const [isOpen, setIsOpen] = React.useState(false);

  const toggleNav = () => {
    setIsOpen(!isOpen);
  };

  const onLogout = async () => {
    await logout();
    router.push("/");
  };

  return (
    <nav className="fixed w-full z-40 top-0 bg-white">
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

          {isLoggedIn ? (
            <>
              <div className="hidden md:block ml-auto">
                {user.role === "RestaurantManager" && (
                  <Link href="/dashboard">
                    <a className="px-2 py-2 uppercase font-medium text-gray-900 hover:text-primary">
                      Dashboard
                    </a>
                  </Link>
                )}
                <button
                  type="button"
                  onClick={onLogout}
                  className="px-2 py-2 uppercase font-medium text-gray-900 hover:text-primary"
                >
                  Logout
                </button>
              </div>
            </>
          ) : (
            <>
              <Link href="/login">
                <a className="hidden md:block ml-auto px-2 py-2 uppercase font-medium text-gray-900 hover:text-primary">
                  Login
                </a>
              </Link>
              <Link href="/register">
                <a className="hidden md:block px-2 py-2 uppercase font-medium text-gray-900 hover:text-primary">
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
          {isLoggedIn ? (
            <>
              <li>
                {user.role === "RestaurantManager" && (
                  <Link href="/dashboard">
                    <a className="block py-2 uppercase font-medium text-gray-900 hover:text-primary">
                      <DashboardIcon className="w-6 h-6 inline" />
                      <span className="ml-2 align-middle">Dashboard</span>
                    </a>
                  </Link>
                )}
                <button
                  type="button"
                  onClick={onLogout}
                  className="block py-2 uppercase font-medium text-gray-900 hover:text-primary"
                >
                  <LogoutIcon className="w-6 h-6 inline" />
                  <span className="ml-2 align-middle">Logout</span>
                </button>
              </li>
            </>
          ) : (
            <>
              <li>
                <Link href="/login">
                  <a className="block py-2 uppercase font-medium text-gray-900 hover:text-primary">
                    <LogoutIcon className="w-6 h-6 inline" />
                    <span className="ml-2 align-middle">Login</span>
                  </a>
                </Link>
              </li>
              <li>
                <Link href="/register">
                  <a className="block py-2 uppercase font-medium text-gray-900 hover:text-primary">
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
