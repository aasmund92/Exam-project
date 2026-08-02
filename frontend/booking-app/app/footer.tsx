export default function Footer() {
    const currentYear = new Date().getFullYear();
    return(
    <footer>
        <div className="w-full mx-auto max-w-screen-xl p-4 md:flex md:items-center md:justify-between">
        <span className="text-sm text-gray-500 sm:text-center dark:text-gray-400">©{currentYear} . All Rights Reserved Clinic Booking App.</span>
        </div>
    </footer>
    );
};