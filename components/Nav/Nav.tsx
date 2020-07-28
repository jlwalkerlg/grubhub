import React, { FC, useState } from "react";
import Link from "next/link";

const Nav: FC = () => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleNav = () => {
    setIsOpen(!isOpen);
  };

  return (
    <nav className="bg-white shadow-md">
      <div className="container px-2 mx-auto flex items-center h-16 py-3">
        <Link href="/">
          <a className="font-medium">
            <span className="material-icons align-middle">restaurant_menu</span>
            <span className="align-middle ml-1">FOOD</span>
            <span className="text-primary align-middle ml-1">SNAP</span>
          </a>
        </Link>

        <button
          className="md:hidden ml-auto rounded w-8 h-8 bg-primary p-1"
          onClick={toggleNav}
        >
          <svg
            className="text-white"
            fill="none"
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            {isOpen ? (
              <path d="M6 18L18 6M6 6l12 12"></path>
            ) : (
              <path d="M4 6h16M4 12h16M4 18h16"></path>
            )}
          </svg>
        </button>

        <div
          className={`md:hidden ${
            isOpen ? "" : "hidden"
          } fixed bottom-0 w-full h-full left-0 px-2 bg-gray-100`}
          style={{ top: "4rem" }}
        >
          <ul className="container mx-auto mt-5">
            <li>
              <Link href="/login">
                <a className="block py-2 uppercase font-medium text-gray-900 hover:text-primary">
                  <span className="material-icons align-middle">login</span>
                  <span className="ml-2 align-middle">Login</span>
                </a>
              </Link>
            </li>
            <li>
              <Link href="/register">
                <a className="block py-2 uppercase font-medium text-gray-900 hover:text-primary">
                  <span className="material-icons align-middle">
                    how_to_reg
                  </span>
                  <span className="ml-2 align-middle">Register</span>
                </a>
              </Link>
            </li>
            <li>
              <div className="mt-5 pt-1 border-solid border-gray-400 border-t-2">
                <Link href="/register">
                  <a className="mt-6 inline-block bg-primary px-4 py-2 rounded text-white uppercase hover:bg-primary-darker">
                    Register Restaurant
                  </a>
                </Link>
              </div>
            </li>
          </ul>
        </div>

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
        <Link href="/register-restaurant">
          <a className="hidden md:block bg-primary px-4 py-2 ml-4 rounded text-white uppercase font-medium hover:bg-primary-darker">
            Register Restaurant
          </a>
        </Link>
      </div>
    </nav>
  );
};

export default Nav;
