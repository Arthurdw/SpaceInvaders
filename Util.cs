// MIT License

// Copyright (c) 2021 Arthurdw

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// ---------------------------------------------------------------------------- //
//                                    ABOUT                                     //
//                                                                              //
//  This project was created for my final exams in GO-AO informatics (6INFO).   //
//  The task was to create a project which is a playable game in the .NET       //
//  framework, which utilizes the MySQL.Data dll.                               //
//                                                                              //
//  The project was finished on the 31'st of may 2021.                          //
//                                                                              //
// ---------------------------------------------------------------------------- //

using System.Security.Cryptography;
using System.Text;

namespace SpaceInvaders
{
    internal class Util
    {
        public static string HashPassword(string pwd, string username)
        {
            pwd = $"5p4c3 1nv4d3r5::{pwd}::{username}";

            byte[] dt = Encoding.UTF8.GetBytes(pwd);
            for (int i = 0; i < dt.Length; i++)
                dt[i] = (byte)(~dt[i] | dt[i] >> 2 & dt[i] << 2);

            using SHA512 sha512Hash = SHA512.Create();
            byte[] bytes = sha512Hash.ComputeHash(dt);

            StringBuilder sb = new StringBuilder();
            foreach (byte t in bytes)
                sb.Append(t.ToString("x"));

            return sb.ToString();
        }
    }
}