import React, { FC } from "react";
import Link from "next/link";

const Nav: FC = () => {
  return (
    <nav className="bg-white shadow-md">
      <div className="container mx-auto flex items-center py-3">
        <Link href="/">
          <a className="font-medium">
            <span className="material-icons align-middle">restaurant_menu</span>
            <span className="align-middle ml-1">FOOD</span>
            <span className="text-red-700 align-middle ml-1">SNAP</span>
          </a>
        </Link>
        <Link href="/login">
          <a className="ml-auto px-2 py-2 uppercase font-medium text-gray-900 hover:text-red-700">
            Login
          </a>
        </Link>
        <Link href="/register">
          <a className="px-2 py-2 uppercase font-medium text-gray-900 hover:text-red-700">
            Register
          </a>
        </Link>
        <Link href="/register-restaurant">
          <a className="bg-red-700 px-4 py-2 ml-4 rounded text-gray-100 uppercase hover:bg-red-800">
            Register Restaurant
          </a>
        </Link>
      </div>
    </nav>
  );
};

export default Nav;
