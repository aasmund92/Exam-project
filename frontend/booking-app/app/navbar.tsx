import Link from "next/link";

export default function NavBar() {
    return(
        <nav className="w-full fixed top-0 left-0 block w-full px-4 py-2 z-50 bg-white shadow-md  lg:px-8 lg:py-3">
            <div className="container flex flex-wrap items-center justify-between mx-auto text-slate-800">
            <Link 
                href="/" 
                className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-200 hover:text-white">
                        <img src="/norofflogo.png" className="h-8" alt="noroffLogo"></img>
            </Link>
        </div>
</nav>
    )
};