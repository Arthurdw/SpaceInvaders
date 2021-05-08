-- phpMyAdmin SQL Dump
-- version 4.6.6deb5ubuntu0.5
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: May 08, 2021 at 01:29 PM
-- Server version: 5.7.33-0ubuntu0.18.04.1
-- PHP Version: 7.2.24-0ubuntu0.18.04.7

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `06InfoArthur`
--

-- --------------------------------------------------------

--
-- Table structure for table `EX2_space_invaders_accounts`
--

CREATE TABLE `EX2_space_invaders_accounts` (
  `id` int(11) NOT NULL,
  `name` varchar(32) NOT NULL,
  `password` varchar(256) NOT NULL,
  `password_raw` varchar(128) NOT NULL,
  `is_admin` tinyint(1) NOT NULL DEFAULT '0',
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `last_seen` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `EX2_space_invaders_accounts`
--

INSERT INTO `EX2_space_invaders_accounts` (`id`, `name`, `password`, `password_raw`, `is_admin`, `created_at`, `last_seen`) VALUES
(3, 'natacha', '*4CEAD9E5BFCE723AACAC8EE02D027E7C1E66E712', 'nb123456', 1, '2021-05-03 14:56:23', '2021-05-03 14:56:23'),
(4, 'admin', '*E98140D5D5455ACE31C19A8D3D3C5A313714BC2B', 'admin', 1, '2021-05-03 14:59:41', '2021-05-04 15:26:08');

-- --------------------------------------------------------

--
-- Table structure for table `EX2_space_invaders_scores`
--

CREATE TABLE `EX2_space_invaders_scores` (
  `id` int(11) NOT NULL,
  `owner` int(11) NOT NULL,
  `score` int(11) NOT NULL,
  `registered_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `EX2_space_invaders_accounts`
--
ALTER TABLE `EX2_space_invaders_accounts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `EX2_space_invaders_scores`
--
ALTER TABLE `EX2_space_invaders_scores`
  ADD PRIMARY KEY (`id`),
  ADD KEY `owner` (`owner`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `EX2_space_invaders_accounts`
--
ALTER TABLE `EX2_space_invaders_accounts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;
--
-- AUTO_INCREMENT for table `EX2_space_invaders_scores`
--
ALTER TABLE `EX2_space_invaders_scores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `EX2_space_invaders_scores`
--
ALTER TABLE `EX2_space_invaders_scores`
  ADD CONSTRAINT `EX2_space_invaders_scores_ibfk_1` FOREIGN KEY (`owner`) REFERENCES `EX2_space_invaders_accounts` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
