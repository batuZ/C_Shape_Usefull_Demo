using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace DotNet.Utilities.获取主机硬件信息
{
    class GetPCInfos
    {
        List<string> str = new List<string>();
        public void getInfos(string savePath)
        {
            str = new List<string>();
            //操作系统信息
            sys();
            //CPU
            CPU();
            //内存
            Memory();
            //显卡
            HardDesk();
        }

        //操作系统信息
        private void sys()
        {
            try
            {
                str.Add($"操作系统：{Environment.OSVersion} {(Environment.Is64BitOperatingSystem ? " X64" : "")}");
                str.Add($"计算机名：{Environment.MachineName}");
                str.Add($"dotNet版本：{Environment.Version}");
            }
            catch
            {
                str.Add("操作系统信息提取失败！");
            }
        }
        //CPU
        private void CPU()
        {
            ManagementObjectCollection cpus = new ManagementObjectSearcher("SELECT * FROM Win32_Processor").Get();
            str.Add($"CPU：数量 {cpus.Count}");
            foreach (ManagementObject _cpu in cpus)
            {
                /** 关键字说明：
                 * Name                     cpu名
                 * CurrentClockSpeed        cpu当前速度 (MHZ)
                 * MaxClockSpeed            cpu最大速度
                 * NumberOfCores            cpu核心数量
                 * NumberOfLogicalProcessors    逻辑处理器数量（线程数？）
                 * L2CacheSize              二级缓存大小
                 * L3CacheSize              三级缓存大小
                 */

                str.Add($"CPU: {_cpu.Properties["Name"].Value}");
                str.Add($"核心数：{_cpu.Properties["NumberOfCores"].Value}");
                str.Add($"线程数：{_cpu.Properties["NumberOfLogicalProcessors"].Value}");
                str.Add($"主频：{_cpu.Properties["CurrentClockSpeed"].Value} 至 {_cpu.Properties["MaxClockSpeed"].Value}");
                str.Add($"缓存： L2 {_cpu.Properties["L2CacheSize"].Value} ; L3 {_cpu.Properties["L3CacheSize"].Value}");
            }
        }
        //内存
        private void Memory()
        {
            ManagementObjectCollection memory = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory").Get();
            double capacity = 0;
            foreach (ManagementObject itm in memory)
                capacity += Convert.ToDouble(itm["Capacity"]);

            str.Add($"内存：{(capacity / 1024 / 1024 / 1024)}");
        }
        //显卡
        private void HardDesk()
        {
            try
            {
                ManagementObjectCollection display = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController").Get();
                str.Add($"显卡数量：{display.Count}");
                foreach (ManagementObject aDis in display)
                {
                    //Caption 型号
                    //AdapterRAM 显存
                    str.Add($"{aDis.Properties["Caption"]} : 显存（{aDis.Properties["AdapterRAM"]}）");
                }
            }
            catch { }
        }
    }
}
