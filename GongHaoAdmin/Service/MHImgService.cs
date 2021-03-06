﻿using GongHaoAdmin.Models;
using GongHaoAdmin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Service
{
    public class MHImgService
    {
        MHImgRepository mr = new MHImgRepository();

        public List<Tab_MHImg> GetMHImgList(int mhid, int pageIndex, int pageSize, out int totalPage, out int totalRecord)
        {
            return mr.GetMHImgList(mhid, pageIndex, pageSize, out totalPage, out totalRecord);
        }

        public int AddImg(Tab_MHImg m)
        {
            return mr.AddImg(m);
        }

        public int GetMaxSort(int mhid)
        {
            return mr.GetMaxSort(mhid);
        }

        public int DeleteImg(int id)
        {
            return mr.DeleteImg(id);
        }

        public Tab_MHImg GetImg(int img)
        {
            return mr.GetImg(img);
        }

        public int UpdateImg(Tab_MHImg m)
        {
            try
            {
                return mr.UpdateImg(m);
            }
            catch (Exception ex)
            {
                return 2;
            }
        }
    }
}